using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoResetEventDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //DemoOne();
            DemoThree();

            Console.ReadLine();
        }

        private static void DemoOne()
        {
            //AutoResetEvent example
            //AutoResetEvent 通知正在等待的线程已发生的事件。
            AutoResetEvent waitHandler = new AutoResetEvent(false);//false 即非终止，未触发。

            new Thread(() =>
            {
                waitHandler.WaitOne();  //阻塞当前线程，等待底层内核对象收到信号。
                Console.WriteLine("线程1接收到信号，开始处理。");

            }).Start();

            new Thread(() =>
            {
                waitHandler.WaitOne();  //阻塞当前线程，等待底层内核对象收到信号。
                Console.WriteLine("线程2接收到信号，开始处理。");

            }).Start();

            new Thread(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("线程3发信号");
                waitHandler.Set();    //向内核对象发送信号。设置事件对象为非终止状态、false，解除阻塞。  

            }).Start();

            //waitHandler.Close(); //释放句柄资源。
            //waitHandler.Reset();  //手动设置事件为非终止状态、false，线程阻止。
            Console.ReadLine();
        }

        private static void DemoTwo()
        {
            //AutoResetEvent实例初始为非终止状态
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            new Thread(() =>
                {
                    while (true)
                    {
                        //调用WaitOne来等待信号，并设置超时时间为5秒
                        bool status = autoResetEvent.WaitOne(5000);
                        if (status)
                        {
                            Console.WriteLine("ThreadOne get the signal");
                        }
                        else
                        {
                            Console.WriteLine("ThreadOne timeout(5 seconds) waiting for signal");
                            break;
                        }
                    }
                    Console.WriteLine("ThreadOne Exit");
                }).Start();

            new Thread(() =>
            {
                while (true)
                {
                    //调用WaitOne来等待信号，并设置超时时间为5秒
                    bool status = autoResetEvent.WaitOne(5000);
                    if (status)
                    {
                        Console.WriteLine("ThreadTwo get the signal");
                    }
                    else
                    {
                        Console.WriteLine("ThreadTwo timeout(5 seconds) waiting for signal");
                        break;
                    }
                }
                Console.WriteLine("ThreadTwo Exit");
            }).Start();

            Random ran = new Random();
            for (int i = 0; i < 8; i++)
            {
                Thread.Sleep(ran.Next(500, 1000));
                //通过Set向 AutoResetEvent 发信号以释放等待线程
                Console.WriteLine("Main thread send the signal");
                autoResetEvent.Set(); //AutoResetEvent.Set一次只能唤醒一个线程，其他线程还是堵塞
            }


            //
            Console.ReadLine();
        }


        private static void DemoThree()
        {
            //AutoResetEvent实例初始为非终止状态
            AutoResetEvent autoResetEvent = new AutoResetEvent(false);

            new Thread(() =>
            {
                while (true)
                {
                    bool status = autoResetEvent.WaitOne();
                   
                    {
                        Console.WriteLine("ThreadOne get the signal" + DateTime.Now.ToString());
                    }
                }
                Console.WriteLine("ThreadOne Exit");
            }).Start();

            new Thread(() =>
            {
                while (true)
                {
                    //调用WaitOne来等待信号，并设置超时时间为5秒
                    bool status = autoResetEvent.WaitOne();
                    {
                        Console.WriteLine("ThreadTwo get the signal #####################");
                    }
                }
                Console.WriteLine("ThreadTwo Exit");
            }).Start();

            Random ran = new Random();
            for (int i = 0; i < 8; i++)
            {
                Thread.Sleep(ran.Next(500, 1000));
                //通过Set向 AutoResetEvent 发信号以释放等待线程
                Console.WriteLine("Main thread send the signal");
                autoResetEvent.Set(); //AutoResetEvent.Set一次只能唤醒一个线程，其他线程还是堵塞
            }

            //
            Console.ReadLine();
        }
    }
}
