using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace BHOHelloWorld
{
    public class LogHelper
    {
        public static readonly Thread WriteThread;
        public static readonly Queue<string> MsgQueue;
        public static readonly object FileLock;

        public static readonly string FilePath;

        static LogHelper()
        {
            FileLock = new object();
            FilePath = "D:\\";
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            WriteThread = new Thread(WriteMsg);
            WriteThread.IsBackground = true;
            MsgQueue = new Queue<string>();
            WriteThread.Start();
        }

        public static void LogInfo(string msg)
        {
            try
            {
                Monitor.Enter(MsgQueue);
                MsgQueue.Enqueue(string.Format("{0} {1} {2} {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff"), "Info", System.Diagnostics.Process.GetCurrentProcess().Id, msg));
                Monitor.Exit(MsgQueue);
            }
            catch { }
        }

        public static void LogError(string msg)
        {
            try
            {
                Monitor.Enter(MsgQueue);
                MsgQueue.Enqueue(string.Format("{0} {1} {2} {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff"), "Error", System.Diagnostics.Process.GetCurrentProcess().Id, msg));
                Monitor.Exit(MsgQueue);
            }
            catch { }
        }

        public static void LogError(string msg, Exception ex)
        {
            try
            {
                Monitor.Enter(MsgQueue);
                MsgQueue.Enqueue(string.Format("{0} {1} {2} {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff"), "Error", System.Diagnostics.Process.GetCurrentProcess().Id, GetExceptionMsg(ex, msg)));
                Monitor.Exit(MsgQueue);
            }
            catch { }
        }

        public static void LogWarn(string msg)
        {
            try
            {
                Monitor.Enter(MsgQueue);
                MsgQueue.Enqueue(string.Format("{0} {1} {2} {3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff"), "Warn", System.Diagnostics.Process.GetCurrentProcess().Id, msg));
                Monitor.Exit(MsgQueue);
            }
            catch { }
        }

        static string GetExceptionMsg(Exception ex, string backStr)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("****************************异常文本****************************");
            sb.AppendLine("【异常信息】：" + backStr);
            sb.AppendLine("【出现时间】：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss.fff"));
            if (ex != null)
            {
                sb.AppendLine("【异常类型】：" + ex.GetType().Name);
                sb.AppendLine("【异常信息】：" + ex.Message);
                sb.AppendLine("【堆栈调用】：" + ex.StackTrace);
            }
            else
            {
                sb.AppendLine("【未处理异常】：" + backStr);
            }
            sb.AppendLine("***************************************************************");
            return sb.ToString();
        }

        private static void WriteMsg()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    //System.IO.File.AppendAllText("c:\\ygs.txt",System.Threading.Thread.CurrentThread.ManagedThreadId + "  Datetime:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff"));
                    if (MsgQueue.Count > 0)
                    {
                        Monitor.Enter(MsgQueue);
                        string msg = MsgQueue.Dequeue();
                        Monitor.Exit(MsgQueue);

                        Monitor.Enter(FileLock);
                        if (!Directory.Exists(FilePath))
                        {
                            Directory.CreateDirectory(FilePath);
                        }
                        string fileName = FilePath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                        var logStreamWriter = new StreamWriter(fileName, true);

                        logStreamWriter.WriteLine(msg);
                        logStreamWriter.Close();
                        Monitor.Exit(FileLock);

                        if (GetFileSize(fileName) > 1024 * 10)
                        {
                            CopyToBak(fileName);
                        }
                    }
                }
                catch
                { }
            }
        }
        private static long GetFileSize(string fileName)
        {
            long strRe = 0;
            if (File.Exists(fileName))
            {
                Monitor.Enter(FileLock);
                var myFs = new FileStream(fileName, FileMode.Open);
                strRe = myFs.Length / 1024;
                myFs.Close();
                myFs.Dispose();
                Monitor.Exit(FileLock);
            }
            return strRe;
        }
        private static void CopyToBak(string sFileName)
        {
            int fileCount = 0;
            string sBakName = "";
            Monitor.Enter(FileLock);
            do
            {
                fileCount++;
                sBakName = sFileName + "." + fileCount + ".BAK";
            }
            while (File.Exists(sBakName));

            File.Copy(sFileName, sBakName);
            File.Delete(sFileName);
            Monitor.Exit(FileLock);
        }
    }
}
