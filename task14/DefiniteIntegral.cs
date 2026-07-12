using System;
using System.Threading;


public class DefiniteIntegral
{
    //
    // a, b - границы отрезка, на котором происходит вычисление опредленного интеграла
    // function - функция, для которой вычисляется определнный интеграл
    // step - размер одного шага разбиения
    // threadsNumber - число потоков, которые используются для вычислений
    //
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {

        double[] Sum = new double[1];
        Sum[0] = 0.0;

        double chunkLength = (b - a) / threadsNumber;

        using Barrier barrier = new Barrier(threadsNumber + 1);


        for (int i = 0; i < threadsNumber; i++)
        {
            int threadIndex = i;

            Thread t = new Thread(() =>
            {
                double start = a + threadIndex * chunkLength;
                
                double end = (threadIndex == threadsNumber - 1) ? b : start + chunkLength;
                
                double localSum = 0.0;

                for (double x = start; x < end; x += step)
                {   
                    double currentStep = Math.Min(step, end - x);
                    
                    localSum += (function(x) + function(x + currentStep)) / 2.0 * currentStep;
                }

                AddDoubleSafely(ref Sum[0], localSum);

                barrier.SignalAndWait();
            });

            t.Start();
        }


        return Sum[0];
    }

    private static void AddDoubleSafely(ref double target, double valueToAdd)
    {
        double initialValue, computedValue;
        do
        {
            initialValue = target;
            computedValue = initialValue + valueToAdd;
            
        }
        while (initialValue != Interlocked.CompareExchange(ref target, computedValue, initialValue));
    }
}