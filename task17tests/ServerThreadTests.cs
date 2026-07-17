using System;
using System.Threading;
using Xunit;

public class CounterCommand : ICommand
{
    public static int ExecutedCount = 0;
    
    public void Execute()
    {
        Thread.Sleep(5);
        Interlocked.Increment(ref ExecutedCount);
    }
}

public class ServerThreadTests
{
    [Fact]
    public void HardStop_ShouldIgnoreRemainingCommands()
    {
        CounterCommand.ExecutedCount = 0;
        var server = new ServerThread();

        server.AddCommand(new CounterCommand());
        server.AddCommand(new HardStopCommand(server));
        server.AddCommand(new CounterCommand());
        server.AddCommand(new CounterCommand());
        
        server.Start();
        server.WaitUntilStopped();
        
        Assert.Equal(1, CounterCommand.ExecutedCount);
    }

    [Fact]
    public void SoftStop_ShouldExecuteAllCommandsCurrentlyInQueue()
    {
        CounterCommand.ExecutedCount = 0;
        var server = new ServerThread();
        
        server.AddCommand(new CounterCommand());
        server.AddCommand(new CounterCommand());
        server.AddCommand(new SoftStopCommand(server));
        server.AddCommand(new CounterCommand()); 
        
        server.Start();
        server.WaitUntilStopped();
        
        Assert.Equal(3, CounterCommand.ExecutedCount);
    }

    [Fact]
    public void StopCommands_WhenCalledFromMainThread_ShouldThrowException()
    {
        var server = new ServerThread();
        server.Start();
        
        var hardStop = new HardStopCommand(server);
        var softStop = new SoftStopCommand(server);

        Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
        Assert.Throws<InvalidOperationException>(() => softStop.Execute());

        server.AddCommand(new HardStopCommand(server));
        server.WaitUntilStopped();
    }
}