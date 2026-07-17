using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

public class LongTaskCommand : ICommand
{
    private int _slicesLeft;
    private readonly IScheduler _scheduler;
    private readonly string _name;
    
    public static List<string> ExecutionLog = new();

    public LongTaskCommand(string name, int slices, IScheduler scheduler)
    {
        _name = name;
        _slicesLeft = slices;
        _scheduler = scheduler;
    }

    public void Execute()
    {
        if (_slicesLeft > 0)
        {
            ExecutionLog.Add(_name);
            _slicesLeft--;

            if (_slicesLeft > 0)
            {
                _scheduler.Add(this);
            }
        }
    }
}

public class ShortTaskCommand : ICommand
{
    public void Execute()
    {
        LongTaskCommand.ExecutionLog.Add("SHORT_TASK");
    }
}

public class SchedulerTests
{
    [Fact]
    public void RoundRobin_ShouldExecuteTasksInAlternatingOrder()
    {
        LongTaskCommand.ExecutionLog.Clear();
        var server = new ServerThread();
        
        var taskA = new LongTaskCommand("A", 3, server.Scheduler);
        var taskB = new LongTaskCommand("B", 3, server.Scheduler);
        
        server.AddCommand(taskA);
        server.AddCommand(taskB);
        server.AddCommand(new SoftStopCommand(server));

        server.Start();
        server.WaitUntilStopped();

        var expectedOrder = new List<string> { "A", "B", "A", "B", "A", "B" };
        Assert.Equal(expectedOrder, LongTaskCommand.ExecutionLog);
    }

    [Fact]
    public void ShortTask_ShouldNotBeBlockedByLongTasks()
    {
        LongTaskCommand.ExecutionLog.Clear();
        var server = new ServerThread();
        
        var taskA = new LongTaskCommand("A", 3, server.Scheduler);
        var shortTask = new ShortTaskCommand();
        
        server.AddCommand(taskA);
        server.AddCommand(shortTask);
        server.AddCommand(new SoftStopCommand(server));

        server.Start();
        server.WaitUntilStopped();

        var expectedOrder = new List<string> { "A", "SHORT_TASK", "A", "A" };
        Assert.Equal(expectedOrder, LongTaskCommand.ExecutionLog);
    }
}