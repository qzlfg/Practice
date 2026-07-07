using System;
using System.IO;
using Xunit;

public class MetadataReaderTests
{
    [Fact]
    public void Main_ShouldPrintWarning_WhenNoArgumentsPassed()
    {
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Program.Main(Array.Empty<string>());

        var output = stringWriter.ToString();
        
        Assert.Contains("Укажите путь к DLL", output);

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }

    [Fact]
    public void Main_ShouldPrintMetadata_WhenValidDllPassed()
    {
        string testDllPath = typeof(Program).Assembly.Location;

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Program.Main(new string[] { testDllPath });

        var output = stringWriter.ToString();

        Assert.Contains("Class:", output);
        Assert.Contains("Program", output);
        Assert.Contains("Method: Main", output);

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
    }
}