using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeCounting.Win32;

namespace TimeCounting
{
    /// <summary>
    /// 精确计时主要有以下几种方式
    /// 1 (毫秒精度)调用WIN API中的GetTickCount
    /// 2 (毫秒精度)调用WIN API中的timeGetTime  （推荐）
    /// 3 (毫秒精度)调用.net自带的方法System.Environment.TickCount
    /// 4 (100纳秒精度)调用WIN API中的QueryPerformanceCounter
    /// 5 (100纳秒精度)使用.net的System.Diagnostics.Stopwatch类    （推荐）
    /// 6 (秒精度)用 DateTime.Now.Ticks
    /// 
    /// 通常我们认为在现代计算机体系结构中，单机内存访问的延时在纳秒数数量级（通常在10ns左右），
    /// 而正常的一次网络通信的延迟在0.1~1ms左右（相当于内存访问延时的105~106倍），
    /// 如此巨大的延时差别，也会影响消息的收发的过程，因此消息丢失和消息延迟变得非常普遍。
    /// 
    /// 
    /// 
    /// </summary>
    class Program
    {
        
        static void Main(string[] args)
        {
            //精度 100纳秒
            HiPerfTimer pt = new HiPerfTimer();
            Console.ReadLine();

            pt.Start();
            //System.Threading.Thread.Sleep(100);           // 需要计时的代码 
            int count = 5;
            pt.Stop();
            Console.WriteLine("HiPerfTimer duration: {0} sec", pt.Duration);
            Console.WriteLine("HiPerfTimer duration: {0} ms", pt.Duration * 1000);
            Console.WriteLine("HiPerfTimer duration: {0} μs", pt.Duration * 1000000);
            Console.WriteLine("HiPerfTimer duration: {0} ns", pt.Duration * 1000000000);

            Console.ReadLine();

            //精度 100纳秒
            // 若要为线程指定处理器关联，请使用 ProcessThread.ProcessorAffinity 方法。
            System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
            myStopwatch.Start();
            //System.Threading.Thread.Sleep(100);           // 需要计时的代码 
            int count2 = 5;
            myStopwatch.Stop();

            var elspsedSeconds = myStopwatch.ElapsedTicks / (decimal)System.Diagnostics.Stopwatch.Frequency;
            Console.WriteLine("Stopwatch duration: {0} sec", elspsedSeconds);
            Console.WriteLine("Stopwatch duration: {0} ms", elspsedSeconds * 1000);
            Console.WriteLine("Stopwatch duration: {0} μs", elspsedSeconds * 1000000);
            Console.WriteLine("Stopwatch duration: {0} ns", elspsedSeconds * 1000000000);

            Console.WriteLine("Stopwatch duration: {0} ms", myStopwatch.ElapsedMilliseconds);


            Console.ReadLine();

            //精度 秒
            var begintime = DateTime.Now.Ticks;
            //System.Threading.Thread.Sleep(100);           // 需要计时的代码 
            int count3 = 5;
            var stoptime = DateTime.Now.Ticks;

            var costTicks = stoptime - begintime;
            Console.WriteLine("DateTime.Now.Ticks duration: {0} sec", costTicks / (double)10000000);
            Console.WriteLine("DateTime.Now.Ticks duration: {0} ms", costTicks / (double)10000);
            Console.WriteLine("DateTime.Now.Ticks duration: {0} μs", costTicks / (double)10);
            Console.WriteLine("DateTime.Now.Ticks duration: {0} ns", costTicks * (double)100);

            Console.ReadLine();
        }
    }
}
