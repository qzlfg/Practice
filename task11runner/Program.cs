using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace task11;

public class Program
{
    public static void Main()
    {
        string code = @"
        public class Calculator: ICalculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => a / b;
        }";

        ICalculator calculator = DynamicCompiler.CompileCalculator(code);

        Console.WriteLine($"Add (10 + 5): {calculator.Add(10, 5)}");
        Console.WriteLine($"Mul (10 * 5): {calculator.Mul(10, 5)}");

    }
}