[AttributeUsage(AttributeTargets.Class)]
public class PluginLoadAttribute: Attribute
{
    public string[] Dependecies { get; private set; }

    public PluginLoadAttribute(params string[] dependencies)
    {
        Dependecies = dependencies;
    }
}