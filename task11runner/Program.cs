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

        DynamicCompiler.CompileCalculator(code);

    }
}