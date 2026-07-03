using System.Reflection;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите путь к DLL");
            return;
        }
        Assembly assembly = Assembly.LoadFrom(args[0]);

        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            if (type.IsClass)
            {
                Console.WriteLine($"Class: {type.Name}");

                var AttributesOfClass = type.GetCustomAttributes();

                foreach (Attribute attribute in AttributesOfClass)
                {
                    Type attr = attribute.GetType();
                    Console.WriteLine($"Attribute of Class : {attr.Name}");
                }


                MethodInfo[] methods = type.GetMethods();
                ConstructorInfo[] constructors = type.GetConstructors();


                foreach (MethodInfo method in methods)
                {
                    Console.WriteLine($"Method: {method.Name}");

                    var AttributesOfMethods = method.GetCustomAttributes();

                    ParameterInfo[] parameters = method.GetParameters();

                    foreach (ParameterInfo parameter in parameters)
                    {
                        Console.WriteLine($"Parametr: {parameter}");
                    }

                    foreach (Attribute attribute in AttributesOfMethods)
                    {
                        var attr = attribute.GetType().Name;
                        Console.WriteLine($"Attribute of Method ({method.Name}): {attr}");
                    }
                }


                foreach (ConstructorInfo constructor in constructors)
                {
                    ParameterInfo[] parameters = constructor.GetParameters();

                    Console.WriteLine($"Constructor: {constructor.Name}");

                    foreach (ParameterInfo parameter in parameters)
                    {
                        Console.WriteLine($"Parameter: {parameter.ParameterType.Name} {parameter.Name}");
                    }

                    var AttributesOfConstructor = constructor.GetCustomAttributes();
                    foreach (Attribute attribute in AttributesOfConstructor)
                    {
                        var attr = attribute.GetType().Name;
                        Console.WriteLine($"Attribute of Constructor ({constructor.Name}): {attr}");
                    }

                }
            }
        }
    }
}