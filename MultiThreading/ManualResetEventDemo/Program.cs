using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ManualResetEventDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //DemoOne();
            DemoTwo();

            Console.ReadLine();
        }

        private static void DemoOne()
        {
            ManualResetEvent manualWaitHandler = new ManualResetEvent(false);//false 即非终止，未触发。

            new Thread(() =>
            {
                Console.WriteLine("线程1-等待信号。");
                manualWaitHandler.WaitOne();  //阻塞当前线程对象，等待信号。
                Console.WriteLine("线程1-接收到信号，开始处理。");

                manualWaitHandler.Reset();  //手动 设置事件对象状态为非终止状态，false。

                manualWaitHandler.WaitOne();  //这里直接阻塞等待无效，因为事件对象还是true，必须手动调reset。
                Console.WriteLine("线程1-第二次接收到信号，开始处理。");

            }).Start();

            new Thread(() =>
            {
                Console.WriteLine("线程2-等待信号。");
                manualWaitHandler.WaitOne();  //阻塞当前线程对象，等待信号。
                Console.WriteLine("线程2-接收到信号，开始处理。");

                manualWaitHandler.Reset();  //手动 设置事件对象状态为非终止状态，false。

                manualWaitHandler.WaitOne();  //这里直接阻塞等待无效，因为事件对象还是true，必须手动调reset。
                Console.WriteLine("线程2-第二次接收到信号，开始处理。");

            }).Start();

            new Thread(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("线程3-发信号");
                manualWaitHandler.Set();    //向事件对象发送ok信号。所有使用manualWaitHandler.WaitOne()的线程都将被唤醒

                Thread.Sleep(2000);
                Console.WriteLine("线程3-第二次发信号");
                manualWaitHandler.Set();
            }).Start();


            Console.ReadLine();
        }

        private static void DemoTwo()
        {
            //ManualResetEvent实例初始为非终止状态
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            new Thread(() =>
            {
                //调用WaitOne来等待信号
                Console.WriteLine("Thread wait the signal - the first time");
                manualResetEvent.WaitOne();
                Console.WriteLine("Thread get the signal - the first time");

                Thread.Sleep(1000);

                Console.WriteLine("Thread wait signal - the second time");
                manualResetEvent.WaitOne();
                Console.WriteLine("Thread get the signal - the second time");

                //调用Reset来以将 ManualResetEvent 置于非终止状态
                Console.WriteLine("Child thread reset ManualResetEvent to non-signaled");
                manualResetEvent.Reset(); //reset 执行前所有的manualResetEvent.WaitOne 都会被唤醒（即Set以后，与Reset之前，所有的WaitOne都被唤醒）

                manualResetEvent.WaitOne();
                Console.WriteLine("Thread get the signal - the third time");

                Console.WriteLine("Child thread reset ManualResetEvent to non-signaled");
                manualResetEvent.Reset();

                //调用WaitOne来等待信号，并设置超时时间为3秒
                manualResetEvent.WaitOne(3000);
                Console.WriteLine("timeout while waiting for signal");
            }).Start();

            //通过Set向 ManualResetEvent 发信号以释放等待线程
            Console.WriteLine("Main thread set ManualResetEvent to signaled");
            manualResetEvent.Set();

            Thread.Sleep(13000);

            Console.WriteLine("Main thread set ManualResetEvent to signaled");
            manualResetEvent.Set();

            Console.ReadLine();
        }
    }
}
