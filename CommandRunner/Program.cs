using System;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices.ObjectiveC;

public class Program
{
    public static void Main()
    {
        string baseDir = AppContext.BaseDirectory;

        string FullPath = Path.Combine(baseDir, "FileSystemCommands.dll");

        Assembly asmfeat = Assembly.LoadFrom(FullPath);

        Type[] allTypes = asmfeat.GetTypes();

        foreach (Type type in allTypes)
        {
            if (typeof(ICommand).IsAssignableFrom(type) && type.IsClass)
            {
                if (type.Name == "DirectorySizeCommand")
                {
                    var testDir = Path.Combine(Path.GetTempPath(), "TestDirSize");

                    if (Directory.Exists(testDir))
                    {
                        Directory.Delete(testDir, true);
                    }

                    Directory.CreateDirectory(testDir);
                    var subDir = Path.Combine(testDir, "SubDir");
                    Directory.CreateDirectory(subDir);

                    File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
                    File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");
                    File.WriteAllText(Path.Combine(subDir, "test3.txt"), "Nested");

                    object DirSizeInstance = Activator.CreateInstance(type, testDir);

                    ICommand command = (ICommand) DirSizeInstance;

                    command.Execute();
                }

                else if (type.Name == "FindFilesCommand")
                {
                    var testDir = Path.Combine(Path.GetTempPath(), "TestDirFind");

                    if (Directory.Exists(testDir))
                    {
                        Directory.Delete(testDir, true);
                    }

                    Directory.CreateDirectory(testDir);
                    var subDir = Path.Combine(testDir, "SubDir");
                    Directory.CreateDirectory(subDir);

                    File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
                    File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");
                    File.WriteAllText(Path.Combine(subDir, "file3.txt"), "Some Text");

                    object FdFilesCommandInstance = Activator.CreateInstance(type, testDir, "*.txt");

                    ICommand command = (ICommand) FdFilesCommandInstance;

                    command.Execute();
                }
            }
        }
    }
}