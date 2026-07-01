using System.Reflection;
using System.Runtime.InteropServices;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type someclass)
    {
        var firstclassattr = someclass.GetCustomAttribute<DisplayNameAttribute>();
        var secondclassattr = someclass.GetCustomAttribute<VersionAttribute>();

        if (firstclassattr != null)
        {
            Console.WriteLine($"{someclass.Name} {firstclassattr.DisplayName}");
        }

        if (secondclassattr != null)
        {
            Console.WriteLine($"{someclass.Name} Version: {secondclassattr.Major}.{secondclassattr.Minor}");
        }


        var properties = someclass.GetProperties();
        var methods = someclass.GetMethods();

        foreach (PropertyInfo proper in properties)
        {
            var attrs = proper.GetCustomAttributes();
            if (attrs != null)
            {
                foreach (var attr in attrs)
                {
                    Console.WriteLine($"{proper.Name} {attr.GetType().Name}");
                }
            }
        }

        foreach (MethodInfo method in methods)
        {
            var attrs = method.GetCustomAttributes();
            if (attrs != null)
            {
                foreach (var attr in attrs)
                {
                    Console.WriteLine($"{method.Name} {attr.GetType().Name}");
                }
            }
        }

    }
}