using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread
{
    private readonly ConcurrentQueue<ICommand> _queue = new();
    
    private readonly IScheduler _scheduler = new RoundRobinScheduler();
    
    private readonly AutoResetEvent _signal = new(false); 
    
    private readonly Thread _thread;
    
    private volatile bool _hardStopRequested = false;
    private volatile bool _softStopRequested = false;

    public int ThreadId => _thread.ManagedThreadId;
    
    public IScheduler Scheduler => _scheduler;

    public ServerThread()
    {
        _thread = new Thread(RunLoop) { IsBackground = true };
    }

    public void Start() => _thread.Start();

    public void AddCommand(ICommand cmd)
    {
        if (!_softStopRequested)
        {
            _queue.Enqueue(cmd);
            _signal.Set();
        }
    }

    internal void TriggerHardStop()
    {
        _hardStopRequested = true;
        _signal.Set(); 
    }

    internal void TriggerSoftStop()
    {
        _softStopRequested = true;
        _signal.Set(); 
    }

    public void WaitUntilStopped() => _thread.Join();

    private void RunLoop()
    {
        while (true)
        {
            if (_hardStopRequested)
                break;

            if (_queue.TryDequeue(out var newCmd))
            {
                ExecuteSafe(newCmd);
                continue;
            }
            if (_scheduler.HasCommand())
            {
                var oldCmd = _scheduler.Select();
                ExecuteSafe(oldCmd);
                continue;
            }

            if (_softStopRequested)
                break;

            _signal.WaitOne();
        }
    }

    private void ExecuteSafe(ICommand cmd)
    {
        try { cmd.Execute(); }
        catch (Exception ex) 
        { 
            Console.WriteLine($"[ExceptionHandler]: Ошибка в {cmd.GetType().Name} -> {ex.Message}"); 
        }
    }
}