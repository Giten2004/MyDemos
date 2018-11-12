using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;

namespace ConsoleApp
{
    class Program
    {
        public static Foobar Target { get; private set; }
        public static MethodInfo Method { get; private set; }
        public static Action<Foobar> Executor { get; private set; }

        private static Action<Foobar> CreateExecutor(MethodInfo method)
        {
            ParameterExpression target = Expression.Parameter(typeof(Foobar), "target");
            Expression expression = Expression.Call(target, method);
            return Expression.Lambda<Action<Foobar>>(expression, target).Compile();
        }

        static Program()
        {
            Target = new Foobar();
            Method = typeof(Foobar).GetMethod("Invoke");
            Executor = CreateExecutor(Method);
        }

        static void Main()
        {
            Console.WriteLine("{0,-10}{1,-12}{2}", "Times", "Reflection", "Expression");
            Test(100000);
            Test(1000000);
            Test(10000000);
        }

        private static void Test(int times)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int i = 0; i < times; i++)
            {
                Method.Invoke(Target, new object[0]);
            }
            long elapsed1 = stopwatch.ElapsedMilliseconds;

            stopwatch.Restart();
            for (int i = 0; i < times; i++)
            {
                Executor(Target);
            }
            long elapsed2 = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("{0,-10}{1,-12}{2}", times, elapsed1, elapsed2);
        }
    }

    public class Foobar
    {
        public void Invoke()
        { }
    }
}
