using System;
using System.Diagnostics;

public class TestCommand : ICommand
{
    private readonly int _id;
    private readonly IScheduler _scheduler;
    private int _counter = 0;
    private readonly int _maxCalls;
    public static readonly System.Collections.Concurrent.ConcurrentBag<(int Id, int CallNumber, double TimeMs)> ExecutionLog = new();
    public static Stopwatch GlobalTimer = new();
    public static System.Threading.CountdownEvent DoneSignal;

    public TestCommand(int id, IScheduler scheduler, int maxCalls = 3)
    {
        _id = id;
        _scheduler = scheduler;
        _maxCalls = maxCalls;
    }

    public void Execute()
    {
        _counter++;
        
        ExecutionLog.Add((_id, _counter, GlobalTimer.Elapsed.TotalMilliseconds));
        
        Console.WriteLine($"Поток {_id} вызов {_counter}");
        
        DoneSignal?.Signal();


        System.Threading.Thread.Sleep(10); 

        if (_counter < _maxCalls)
        {
            _scheduler.Add(this);
        }
    }
}