public class SoftStopCommand : ICommand
{
    private readonly ServerThread _server;

    public SoftStopCommand(ServerThread server) => _server = server;

    public void Execute()
    {
        if (Thread.CurrentThread.ManagedThreadId != _server.ThreadId)
            throw new InvalidOperationException("SoftStop можно вызвать только внутри серверного потока!");
        
        _server.TriggerSoftStop();
    }
}