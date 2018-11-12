using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace TimeCounting.Win32
{
    internal class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long _startTime;
        private long _stopTime;
        private long _freq;
        
        public HiPerfTimer()
        {
            _startTime = 0;
            _stopTime = 0;

            if (QueryPerformanceFrequency(out _freq) == false)
            {
                // 不支持高性能计数器 
                throw new Win32Exception();
            }
        }

        // 开始计时器 
        public void Start()
        {
            // 来让等待线程工作 
            Thread.Sleep(0);

            QueryPerformanceCounter(out _startTime);
        }

        // 停止计时器 
        public void Stop()
        {
            QueryPerformanceCounter(out _stopTime);
        }

        // 返回计时器经过时间(单位：秒) 
        public double Duration
        {
            get
            {
                return (double)(_stopTime - _startTime) / (double)_freq;
            }
        }
    }
}
