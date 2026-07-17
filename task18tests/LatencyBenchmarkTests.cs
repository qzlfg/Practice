using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using ScottPlot;
using Xunit;

public class DiagnosticShortTask : ICommand
{
    public double LatencyMs { get; private set; }
    private readonly Stopwatch _globalTimer;
    private readonly double _createdAtMs;

    public DiagnosticShortTask(Stopwatch globalTimer)
    {
        _globalTimer = globalTimer;
        _createdAtMs = _globalTimer.Elapsed.TotalMilliseconds;
    }

    public void Execute()
    {
        LatencyMs = _globalTimer.Elapsed.TotalMilliseconds - _createdAtMs;
    }
}

public class SlicedHeavyTask : ICommand
{
    private int _slicesLeft;
    private readonly IScheduler _scheduler;

    public SlicedHeavyTask(int slices, IScheduler scheduler)
    {
        _slicesLeft = slices;
        _scheduler = scheduler;
    }

    public void Execute()
    {
        Thread.Sleep(10);
        _slicesLeft--;
        if (_slicesLeft > 0)
            _scheduler.Add(this);
    }
}

public class MonolithicHeavyTask : ICommand
{
    private readonly int _totalWorkMs;
    public MonolithicHeavyTask(int totalWorkMs) => _totalWorkMs = totalWorkMs;

    public void Execute()
    {
        Thread.Sleep(_totalWorkMs);
    }
}


public class LegacyServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new();
    private readonly Thread _thread;

    public LegacyServerThread()
    {
        _thread = new Thread(() =>
        {
            foreach (var cmd in _queue.GetConsumingEnumerable()) cmd.Execute();
        }) { IsBackground = true };
        _thread.Start();
    }
    public void AddCommand(ICommand cmd) => _queue.Add(cmd);
    public void StopAndWait()
    {
        _queue.CompleteAdding();
        _thread.Join();
    }
}

public class LatencyBenchmarkTests
{
    [Fact]
    public void GenerateLatencyComparisonChart()
    {
        int shortTasksCount = 10;
        double[] taskIds = new double[shortTasksCount];
        double[] legacyLatencies = new double[shortTasksCount];
        double[] newLatencies = new double[shortTasksCount];


        var legacyServer = new LegacyServerThread();
        var timer1 = Stopwatch.StartNew();

        legacyServer.AddCommand(new MonolithicHeavyTask(500));

        var legacyShortTasks = new List<DiagnosticShortTask>();
        for (int i = 0; i < shortTasksCount; i++)
        {
            Thread.Sleep(30);
            var task = new DiagnosticShortTask(timer1);
            legacyShortTasks.Add(task);
            legacyServer.AddCommand(task);
        }
        
        legacyServer.StopAndWait();
        
        for (int i = 0; i < shortTasksCount; i++)
        {
            taskIds[i] = i + 1;
            legacyLatencies[i] = legacyShortTasks[i].LatencyMs;
        }

        var newServer = new ServerThread();
        newServer.Start();
        var timer2 = Stopwatch.StartNew();

        newServer.AddCommand(new SlicedHeavyTask(50, newServer.Scheduler));

        var newShortTasks = new List<DiagnosticShortTask>();
        for (int i = 0; i < shortTasksCount; i++)
        {
            Thread.Sleep(30); 
            var task = new DiagnosticShortTask(timer2);
            newShortTasks.Add(task);
            newServer.AddCommand(task);
        }

        newServer.AddCommand(new SoftStopCommand(newServer));
        newServer.WaitUntilStopped();

        for (int i = 0; i < shortTasksCount; i++)
        {
            newLatencies[i] = newShortTasks[i].LatencyMs;
        }

        var plt = new Plot();

        var oldScatter = plt.Add.Scatter(taskIds, legacyLatencies);
        oldScatter.LegendText = "Старая архитектура (Без планировщика)";
        oldScatter.LineWidth = 3;
        oldScatter.MarkerSize = 8;

        var newScatter = plt.Add.Scatter(taskIds, newLatencies);
        newScatter.LegendText = "Новая архитектура (Round Robin)";
        newScatter.LineWidth = 3;
        newScatter.MarkerSize = 8;

        plt.Title("Отзывчивость сервера: Задержка выполнения быстрых задач");
        plt.XLabel("Порядковый номер быстрой задачи");
        plt.YLabel("Время ожидания в очереди (мс)");
        plt.ShowLegend();

        string path = Path.GetFullPath("../../../latency_comparison.png");
        plt.SavePng(path, 800, 500);

        Assert.True(newLatencies[shortTasksCount - 1] < 100, "Ошибка: Планировщик тормозит!");
    }
}