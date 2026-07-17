public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ICommand> _tasks = new();

    public bool HasCommand() => _tasks.Count > 0;

    public ICommand Select() => _tasks.Dequeue();

    public void Add(ICommand cmd) => _tasks.Enqueue(cmd);
}