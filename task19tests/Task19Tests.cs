using System;
using System.IO;
using System.Linq;
using System.Threading;
using ScottPlot;
using Xunit;


public class Task19Runner
{
    [Fact]
    public void Execute_5_Commands_3_Times_And_Generate_Report()
    {
        TestCommand.ExecutionLog.Clear();
        TestCommand.GlobalTimer.Restart();
        
        int totalExpectedCalls = 15;
        TestCommand.DoneSignal = new CountdownEvent(totalExpectedCalls);
        
        var server = new ServerThread();

        for (int i = 1; i <= 5; i++)
        {
            server.AddCommand(new TestCommand(i, server.Scheduler, maxCalls: 3));
        }

        server.Start();

        bool completedInTime = TestCommand.DoneSignal.Wait(TimeSpan.FromSeconds(5));
        Assert.True(completedInTime, "Команды не успели выполниться за отведенное время!");

        server.AddCommand(new HardStopCommand(server));
        server.WaitUntilStopped();

        Assert.Equal(totalExpectedCalls, TestCommand.ExecutionLog.Count);

        GenerateReportAndGraph();
    }

    private void GenerateReportAndGraph()
    {
        var sortedLogs = TestCommand.ExecutionLog.OrderBy(x => x.TimeMs).ToList();
        var plt = new Plot();

        for (int i = 1; i <= 5; i++)
        {
            var taskLogs = sortedLogs.Where(x => x.Id == i).ToList();
            
            List<double> xs = new() { 0 };
            List<double> ys = new() { 0 };

            foreach (var log in taskLogs)
            {
                xs.Add(log.TimeMs);
                ys.Add(log.CallNumber);
            }

            var scatter = plt.Add.Scatter(xs.ToArray(), ys.ToArray());
            scatter.LegendText = $"Команда {i}";
            scatter.LineWidth = 3;
            scatter.MarkerSize = 7;
        }

        plt.Title("Прогресс выполнения длительных задач (Round Robin)");
        plt.XLabel("Время выполнения (мс)");
        plt.YLabel("Количество выполненных срезов (из 3)");
        plt.ShowLegend();
        
        string plotPath = Path.GetFullPath("../../../task19_progress.png");
        plt.SavePng(plotPath, 700, 450);

    }
}