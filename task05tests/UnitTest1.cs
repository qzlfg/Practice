using Xunit;
using System;
using System.Linq;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
    
    public void TargetMethod(int id, string name) { } 
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
        Assert.Contains("TargetMethod", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetProperties_ReturnsCorrectProperties()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var properties = analyzer.GetProperties();

        Assert.Contains("Property", properties);
    }

    [Fact]
    public void HasAttribute_WhenAttributeExists_ReturnsTrue()
    {
        var analyzer = new ClassAnalyzer(typeof(AttributedClass));
        bool result = analyzer.HasAttribute<SerializableAttribute>();

        Assert.True(result);
    }

    [Fact]
    public void HasAttribute_WhenAttributeDoesNotExist_ReturnsFalse()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        bool result = analyzer.HasAttribute<SerializableAttribute>();

        Assert.False(result);
    }

    [Fact]
    public void GetMethodParams_ReturnsCorrectParamsAndReturnType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        
        var result = analyzer.GetMethodParams("TargetMethod").ToList();

        Assert.Contains("Int32 value: id", result);

        Assert.Contains("String value: name", result);

        Assert.Contains("ReturnType: Void", result);
    }

    [Fact]
    public void GetMethodParams_WhenMethodNotFound_ReturnsEmpty()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        
        var result = analyzer.GetMethodParams("FakeMethod");

        Assert.Empty(result);
    }

    [Fact]
    public void GetMethodParams_WhenMethodHasNoParams_ReturnsOnlyReturnType()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        
        var result = analyzer.GetMethodParams("Method").ToList();

        Assert.Single(result);

        Assert.Equal("ReturnType: Void", result[0]);
    }
}