using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LockDemo
{
    class PrintNum
    {
        private object _lockObj = new object();

        public void PrintOddNum()
        {
            lock (_lockObj)
            {
                Console.WriteLine("Print Odd numbers:");
                for (int i = 0; i < 10; i++)
                {
                    if (i % 2 != 0)
                        Console.Write(i);
                    Thread.Sleep(100);
                }
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintNum printNum = new PrintNum();
            for (int i = 0; i < 3; i++)
            {
                Thread temp = new Thread(new ThreadStart(printNum.PrintOddNum));
                temp.Start();
            }

            lock (printNum)
            {
                Console.WriteLine("Main thread will delay 5 seconds");

                Thread.Sleep(5000);
                Console.WriteLine("Main thread already delayed 5 seconds, Press any key to exit.");
            }

            Console.ReadLine();
        }
    }
}
