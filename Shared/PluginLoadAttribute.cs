[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute: Attribute
{
    public string[] Dependencies { get; private set; }

    public PluginLoadAttribute(params string[] dependencies)
    {
        Dependencies = dependencies;
    }
}