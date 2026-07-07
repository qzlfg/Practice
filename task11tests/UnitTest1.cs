using System;
using Xunit;
using task11;
public class CalculatorTests
{
    private readonly ICalculator _calculator;

    public CalculatorTests()
    {
        string sourceCode = @"
        using task11;
        public class Calculator : ICalculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

        _calculator = DynamicCompiler.CompileCalculator(sourceCode);
    }

    [Fact]
    public void Add_ShouldReturnCorrectSum()
    {
        int result = _calculator.Add(10, 5);
        
        Assert.Equal(15, result);
    }

    [Fact]
    public void Minus_ShouldReturnCorrectDifference()
    {
        int result = _calculator.Minus(10, 5);
        Assert.Equal(5, result);
    }

    [Fact]
    public void Mul_ShouldReturnCorrectProduct()
    {
        int result = _calculator.Mul(10, 5);
        Assert.Equal(50, result);
    }

    [Fact]
    public void Div_ShouldReturnCorrectQuotient()
    {
        int result = _calculator.Div(10, 5);
        Assert.Equal(2, result);
    }

    [Fact]
    public void Div_ByZero_ShouldThrowException()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Div(10, 0));
    }
}