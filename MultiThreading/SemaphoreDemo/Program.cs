using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SemaphoreDemo
{
    class Program
    {
        //创建信号量实例，
        private static Semaphore _semaphore;

        static void Main(string[] args)
        {
            bool createdNew;
            _semaphore = new Semaphore(0, 3, "SemaphoreTest", out createdNew);

            if (createdNew)
            {
                Thread.Sleep(1000);
                //退出信号量三次，并且计数加三
                _semaphore.Release(3);
            }

            for (int i = 0; i < 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Worker));
                t.Start(i);
            }

            Console.Read();
        }

        private static void Worker(object index)
        {
            //调用WaitOne来等待信号
            _semaphore.WaitOne();

            Console.WriteLine("---> Thread {0} enter Critical code section", index.ToString());
            Random ran = new Random();
            Thread.Sleep(ran.Next(1000, 2000));

            Console.WriteLine("     Thread {0} exit Critical code section <---", index.ToString());
            //退出信号量，并且计数加一
            _semaphore.Release();

        }
    }
}
