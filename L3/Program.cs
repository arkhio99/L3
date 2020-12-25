using System;
using System.Collections.Generic;
using System.Linq;

namespace L3
{
    class Program
    {
        static Random rand = new Random();

        public static double GetRandomValueBetweenTwoNumbers(double a, double b)
        {
            return a + (b - a) * rand.NextDouble();
        }

        public static double GetW(Func<double, double> f, double a, double b, int n, IEnumerable<double> xs)
        {
            double sum = 0;
            foreach (var x in xs)
            {
                sum += f(x);
            }
            return (b - a) * sum / n;
        }

        public static (double, double) GetTrustedInterval(double w, double s, double n)
        {
            double z = 1.96;
            double delt = z * s / Math.Sqrt(n);
            return (w - delt, w + delt);
        }

        static void Main(string[] args)
        {
            // 19 страница 2295
            int n = 100000;
            double b = Math.PI;
            double a = 0;
            Func<double, double> f = (x) => Math.Sin(x) * Math.Cos(3) * x;
            double ideal = -3.11014921124052;
            Console.WriteLine($"Функция: f(x) = sin(x) * cos(3) * x");
            Console.WriteLine($"I = {ideal}");

            // Вычисление
            var xs = Enumerable.Range(1, n).Select(_ => GetRandomValueBetweenTwoNumbers(b, a));

            var w = GetW(f, a, b, n, xs);
            Console.WriteLine($"w = {w}");

            var absoluteDifference = Math.Abs(ideal - w);
            Console.WriteLine($"1) |I - w| = {absoluteDifference}");

            var variance = StatLib.Statistics.GetDispersion(xs, n, w);
            Console.WriteLine($"2) S^2 = {variance}");

            var (leftEdge, rightEdge) = GetTrustedInterval(w, variance, n);
            Console.WriteLine($"3) Доверительный интервал: ({leftEdge}, {rightEdge})");            
        }
    }
}
