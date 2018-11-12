using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousOperations
{
    /// <summary>
    /// https://msdn.microsoft.com/zh-cn/library/system.iasyncresult(v=vs.110).aspx
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // The asynchronous method puts the thread id here.
            int threadId;

            // Create an instance of the test class.
            AsyncDemo ad = new AsyncDemo();

            // Create the delegate.
            AsyncDemo.AsyncMethodCaller caller = new AsyncDemo.AsyncMethodCaller(ad.TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000, out threadId, null, null);

            Thread.Sleep(0);

            Console.WriteLine("Main thread {0} does some work.", Thread.CurrentThread.ManagedThreadId);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            // Perform additional processing here.
            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            Console.WriteLine("The call executed on thread {0}, with return value \"{1}\".", threadId, returnValue);

            Console.ReadLine();
        }
    }
}
