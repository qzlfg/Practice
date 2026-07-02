public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        command.Execute(); 

        var output = stringWriter.ToString();

        Assert.Contains("10", output); 

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        command.Execute(); 

        var output = stringWriter.ToString();
        
        Assert.Contains("file1.txt", output);

        Assert.DoesNotContain("file2.log", output);

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void CommandRunner_Main_ShouldSuccessfullyLoadAndRunPlugins()
    {
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Program.Main();

        var output = stringWriter.ToString();

        Assert.Contains("Размер каталога составляет", output);
        
        Assert.Contains("file1.txt", output);

        
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
}
