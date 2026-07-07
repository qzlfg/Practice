[PluginLoad("PluginB")]
public class PluginA: ICommand
{
    public void Execute()
    {
        Console.WriteLine("Plugin A что-то выполняет");
    }
}
