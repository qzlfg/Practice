using System;
using System.IO;
using System.Reflection;
using System.Collections;

public class Program
{
    public static void Main()
    {
        string basePath = AppContext.BaseDirectory;

        string[] allDllPaths = Directory.GetFiles(basePath, "*.dll");

        Queue<Type> queue = new Queue<Type>();

        Dictionary<Type, int> InDegree = new Dictionary<Type, int>();

        Dictionary<string, List<Type>> graph = new Dictionary<string, List<Type>>();

        foreach (string dllPath in allDllPaths)
        {
            Assembly assembly = Assembly.LoadFrom(dllPath);

            foreach (Type type in assembly.GetTypes())
            {
                bool isClass = type.IsClass;

                bool hasInterface = typeof(ICommand).IsAssignableFrom(type);
                
                var hasAttribute = type.GetCustomAttribute<PluginLoadAttribute>();

                if (isClass && hasInterface && hasAttribute != null)
                {
                    InDegree[type] = hasAttribute.Dependecies.Length;
                    foreach (var dependent_object in hasAttribute.Dependecies)
                    {
                        if (!graph.TryGetValue(dependent_object, out var list))
                        {
                            list = new List<Type>();
                            graph[dependent_object] = list;
                        }
                        list.Add(type);
                    }
                }
            }
        }


        

        foreach (var kvp in InDegree)
        {
            if (kvp.Value == 0)
            {
                queue.Enqueue(kvp.Key);
            }
        }

        int ExecutedCount = 0;

        while (queue.Count > 0)
        {
            Type currentPlugin = queue.Dequeue();

            object instance = Activator.CreateInstance(currentPlugin);

            ICommand command = (ICommand) instance;
            
            command.Execute();

            ExecutedCount++;


            if (graph.ContainsKey(currentPlugin.Name))
            {
                foreach (Type dependent_object in graph[currentPlugin.Name])
                {
                    InDegree[dependent_object]--;

                    if (InDegree[dependent_object] == 0)
                    {
                        queue.Enqueue(dependent_object);
                    }
                }
            }
            
        }

        if (ExecutedCount < InDegree.Count)
        {
            Console.WriteLine("Есть цикл зависимостей плагинов");
        }

    }
}