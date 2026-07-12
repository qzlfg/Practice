using System;
using System.Diagnostics;
using System.IO;
using ScottPlot;
using Xunit;
using Xunit.Abstractions;

public class BenchmarkTests
{
    private readonly ITestOutputHelper _output;

    public BenchmarkTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Task15_PerformanceBenchmarkAndReportGeneration()
    {
        Func<double, double> SIN = Math.Sin;
        double a = -100;
        double b = 100;
        double expectedIntegral = 0.0;
        double requiredAccuracy = 1e-4;

        double[] testSteps = {1e-3, 1e-4, 1e-5, 1e-6 };
        
        double optimalStep = testSteps[0];
        int maxThreads = Environment.ProcessorCount;

        foreach (var step in testSteps)
        {
            double result = DefiniteIntegral.SolveSingleThread(a, b, SIN, step);
            if (Math.Abs(result - expectedIntegral) <= requiredAccuracy)
            {
                optimalStep = step;
                break;
            }
        }

        _output.WriteLine($"Определен оптимальный шаг: {optimalStep}");
        _output.WriteLine("\nБазовое время однопотока");
        
        int runsPerTest = 10;
        double singleThreadTotalTime = 0;
        
        DefiniteIntegral.SolveSingleThread(a, b, SIN, optimalStep); 
        
        for (int i = 0; i < runsPerTest; i++)
        {
            var sw = Stopwatch.StartNew();
            DefiniteIntegral.SolveSingleThread(a, b, SIN, optimalStep);
            sw.Stop();
            singleThreadTotalTime += sw.Elapsed.TotalMilliseconds;
        }
        double singleThreadTime = singleThreadTotalTime / runsPerTest;
        _output.WriteLine($"Однопоточная версия: {singleThreadTime:F4} мс\n");

        _output.WriteLine("Бенчмарк многопотока и Ускорение");
        
        double[] threadCounts = new double[maxThreads];
        double[] executionTimes = new double[maxThreads];
        double bestMultiThreadTime = double.MaxValue;
        int optimalThreads = 1;

        for (int threads = 1; threads <= maxThreads; threads++)
        {
            double totalTimeMs = 0;
            
            DefiniteIntegral.Solve(a, b, SIN, optimalStep, threads);
            
            for (int i = 0; i < runsPerTest; i++)
            {
                var sw = Stopwatch.StartNew();
                DefiniteIntegral.Solve(a, b, SIN, optimalStep, threads);
                sw.Stop();
                totalTimeMs += sw.Elapsed.TotalMilliseconds;
            }

            double avgTime = totalTimeMs / runsPerTest;
            threadCounts[threads - 1] = threads;
            executionTimes[threads - 1] = avgTime;

            double speedup = ((singleThreadTime - avgTime) / singleThreadTime) * 100;

            _output.WriteLine($"Потоков: {threads,2} (+ главный поток) | Время: {avgTime,8:F4} мс | Ускорение: {speedup,6:F2}%");

            if (avgTime < bestMultiThreadTime)
            {
                bestMultiThreadTime = avgTime;
                optimalThreads = threads;
            }
        }

        double finalSpeedup = ((singleThreadTime - bestMultiThreadTime) / singleThreadTime) * 100;

        var plt = new Plot();

        var mtScatter = plt.Add.Scatter(executionTimes, threadCounts);
        mtScatter.LegendText = "Многопоточная версия";

        double[] stTimeArray = { singleThreadTime };
        double[] stThreadArray = { 0 }; 
        var stScatter = plt.Add.Scatter(stTimeArray, stThreadArray);
        stScatter.LegendText = "Чистый однопоток";

        var baseline = plt.Add.VerticalLine(singleThreadTime);

        plt.ShowLegend();
        plt.Title("Зависимость времени от числа потоков");
        plt.XLabel("Время выполнения (мс)");
        plt.YLabel("Количество созданных потоков (0 = чистый цикл)");
        
        plt.SavePng(Path.GetFullPath("../../../benchmark_plot.png"), 600, 400);

        Assert.True(finalSpeedup >= 15.0, $"Ошибка: Ускорение всего {finalSpeedup:F2}%.");
    }
}