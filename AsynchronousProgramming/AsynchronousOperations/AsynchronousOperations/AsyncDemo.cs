using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousOperations
{
    public class AsyncDemo
    {
        // The delegate must have the same signature as the method
        // it will call asynchronously.
        public delegate string AsyncMethodCaller(int callDuration, out int threadId);

        // The method to be executed asynchronously.
        public string TestMethod(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return String.Format("My call time was {0}.", callDuration.ToString());
        }
    }
    
}
