using System.Reflection;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type someclass)
    {
        var FirstClassAttr = someclass.GetCustomAttribute<DisplayNameAttribute>();
        var SecondClassAttr = someclass.GetCustomAttribute<VersionAttribute>();

        if (FirstClassAttr != null)
        {
            Console.WriteLine($"{someclass.Name} {FirstClassAttr.DisplayName}");
        }

        if (SecondClassAttr != null)
        {
            Console.WriteLine($"{someclass.Name} Version: {SecondClassAttr.Major}.{SecondClassAttr.Minor}");
        }


        var properties = someclass.GetProperties();
        var methods = someclass.GetMethods();

        foreach (PropertyInfo proper in properties)
        {
            var AttributesOfProperty = proper.GetCustomAttributes();
            if (AttributesOfProperty != null)
            {
                foreach (var AttributeOfProperty in AttributesOfProperty)
                {
                    Console.WriteLine($"{proper.Name} {AttributeOfProperty.GetType().Name}");
                }
            }
        }

        foreach (MethodInfo method in methods)
        {
            var AttributesOfMethod = method.GetCustomAttributes();
            if (AttributesOfMethod != null)
            {
                foreach (var AttributeOfMethod in AttributesOfMethod )
                {
                    Console.WriteLine($"{method.Name} {AttributeOfMethod.GetType().Name}");
                }
            }
        }

    }
}