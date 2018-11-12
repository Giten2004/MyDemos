using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MutexDemo
{
    class Program
    {
        private static Mutex _mutex;

        static void Main(string[] args)
        {
            string mutexName = "MutexTest";

            try
            {
                //尝试打开已有的互斥体
                _mutex = Mutex.OpenExisting(mutexName);
            }
            catch (WaitHandleCannotBeOpenedException e)
            {
                Console.WriteLine("Mutex named {0} is not exist, error message: {1}", mutexName, e.Message);
                //Mutex实例初始为非终止状态
                _mutex = new Mutex(false, mutexName);
                Console.WriteLine("Create Mutex {0}", mutexName);
            }

            for (int i = 0; i < 10; i++)
            {
                //通过线程池调用时间打印函数
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetCurrentTime));
            }

            Console.ReadLine();
        }
        static void GetCurrentTime(object state = null)
        {
            ///
            /// 互斥体Mutex也有终止和非终止状态，调用ReleaMutex 后互斥体的状态设定为终止，直到其他线程占有互斥体，但是如果没有线程拥有互斥体的话，
            /// 该互斥体的状态将一直保持终止状态。

            ///注意：WaitOne方法和ReleaseMutex方法的使用次数必须一致。
            /// 前面介绍过互斥体Mutex允许同一个线程多次重复访问共享区，也就是说，拥有互斥体的线程可以在对 WaitOne 的重复调用中请求相同的互斥体而不会阻止其执行， 
            /// 但线程必须调用 ReleaseMutex 方法同样多的次数以释放互斥体的所属权。

            //调用WaitOne来等待信号
            _mutex.WaitOne();
            try
            {
                //Mutex支持同一个线程多次重复访问共享区，所以加上下面的WaitOne一样可以工作
                //mutex.WaitOne();
                Thread.Sleep(1000);
                Console.WriteLine("Current Time is: {0}", DateTime.Now);
                //mutex.ReleaseMutex();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }

        private static void CheckInstance()
        {
            //由于命名的Mutex可以跨越进程，所以Mutex还有一个常用的场景就是用来限制一次只能运行一个程序实例。
            bool canCreateNew;

            //通过canCreateNew来判断是否是新建的互斥锁实例
            Mutex m = new Mutex(true, "SingleApp", out canCreateNew);
            if (canCreateNew)
            {
                Console.WriteLine("Start App to print time");
                new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Current time is {0}", DateTime.Now);
                    }
                }).Start();
            }
            else
            {
                Console.WriteLine("App already start");
            }

            Console.Read();
        }
    }
}
