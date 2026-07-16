public class HardStopCommand : ICommand
{
    private readonly ServerThread _server;

    public HardStopCommand(ServerThread server) => _server = server;

    public void Execute()
    {
        if (Thread.CurrentThread.ManagedThreadId != _server.ThreadId)
            throw new InvalidOperationException("HardStop можно вызвать только внутри серверного потока!");
        
        _server.TriggerHardStop();
    }
}