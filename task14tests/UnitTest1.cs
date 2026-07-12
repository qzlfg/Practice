using System;
using Xunit;

public class DefiniteIntegralTests
{
    [Fact]
    public void Solve_UserProvidedTests_ShouldPass()
    {
        var X = (double x) => x;
        var SIN = (double x) => Math.Sin(x);

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);

        Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
    }

    [Fact]
    public void Solve_ConstantFunction_ShouldReturnAreaOfRectangle()
    {
        Func<double, double> CONST = (double x) => 5.0;
        
        double actual = DefiniteIntegral.Solve(0, 10, CONST, 1e-4, 4);
        
        Assert.Equal(50.0, actual, 3);
    }

    [Fact]
    public void Solve_ExponentialFunction_ShouldCalculateCorrectly()
    {
        Func<double, double> EXP = (double x) => Math.Exp(x);
        double expected = Math.E - 1;
        
        double actual = DefiniteIntegral.Solve(0, 1, EXP, 1e-5, 6);
        
        Assert.Equal(expected, actual, 4);
    }

    [Fact]
    public void Solve_CosineFunction_ShouldHandlePositiveAndNegativeWaves()
    {
        Func<double, double> COS = (double x) => Math.Cos(x);
        double expected = 2.0;
        
        double actual = DefiniteIntegral.Solve(-Math.PI / 2, Math.PI / 2, COS, 1e-5, 8);
        
        Assert.Equal(expected, actual, 4);
    }


    [Fact]
    public void Solve_SingleThread_ShouldWorkWithoutConcurrencyIssues()
    {
        Func<double, double> parabola = (double x) => x * x;
        double expected = 9.0;
        
        double actual = DefiniteIntegral.Solve(0, 3, parabola, 1e-4, 1);
        
        Assert.Equal(expected, actual, 3);
    }
    
    [Fact]
    public void Solve_HighThreadCount_ShouldDistributeWorkCorrectly()
    {
        Func<double, double> X3 = (double x) => x * x * x;
        double expected = 64.0;
        
        double actual = DefiniteIntegral.Solve(0, 4, X3, 1e-5, 16);
        
        Assert.Equal(expected, actual, 3);
    }
}