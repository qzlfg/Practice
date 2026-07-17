using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread
{
    private readonly BlockingCollection<ICommand> _queue = new();
    private readonly Thread _thread;
    private volatile bool _hardStopRequested = false;

    public int ThreadId => _thread.ManagedThreadId;

    public ServerThread()
    {
        _thread = new Thread(RunLoop);
        _thread.IsBackground = true;
    }

    public void Start() => _thread.Start();

    public void AddCommand(ICommand cmd)
    {
        if (!_queue.IsAddingCompleted)
        {
            _queue.Add(cmd);
        }
    }

    internal void TriggerHardStop()
    {
        _hardStopRequested = true;
    }

    internal void TriggerSoftStop()
    {
        _queue.CompleteAdding(); 
    }

    public void WaitUntilStopped() => _thread.Join();

    private void RunLoop()
    {
        foreach (var cmd in _queue.GetConsumingEnumerable())
        {
            try
            {
                cmd.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ExceptionHandler]: Ошибка в {cmd.GetType().Name} -> {ex.Message}");
            }

            if (_hardStopRequested)
            {
                break; 
            }
        }
    }
}
