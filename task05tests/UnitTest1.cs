using Xunit;
using Moq;

public class TestClass
{
    public int PublicField;
    private string _privateField;
    public int Property { get; set; }

    public void Method() { }
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
}