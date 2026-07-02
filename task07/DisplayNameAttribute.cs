public class DisplayNameAttribute: Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string name)
    {
        DisplayName = name;
    }

}
