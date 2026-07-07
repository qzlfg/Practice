using System;
using System.Collections.Generic;
using Xunit;

public class PluginIntegrationTests
{
    [Fact]
    public void Main_ShouldExecutePluginsInCorrectOrder()
    {
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        Program.Main();

        string actualOutput = stringWriter.ToString();

        Assert.Contains("Plugin A что-то выполняет", actualOutput);
        Assert.Contains("Плагин B что-то выполяет", actualOutput);

        int indexA = actualOutput.IndexOf("Plugin A что-то выполняет");
        int indexB = actualOutput.IndexOf("Плагин B что-то выполяет");

        Assert.True(indexB < indexA, "Ошибка: PluginB должен был отработать раньше, чем PluginA!");
    }

    [Fact]
    public void Main_OutputShouldNotBeEmptyAndContainExecutionLogs()
    {
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var exception = Record.Exception(() => Program.Main());
        
        Assert.Null(exception); 

        string actualOutput = stringWriter.ToString();

        Assert.NotNull(actualOutput);
        Assert.NotEmpty(actualOutput);
    }
}