using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubSub
{
    class Program
    {
        static void Main(string[] args)
        {
            var pub = new Pub();
            var sub = new Sub();

            var pubThread = new System.Threading.Thread(pub.Init);
            var subThread = new System.Threading.Thread(sub.Init);

            pubThread.Start();
            Thread.Sleep(10000);
            subThread.Start();

            Console.ReadLine();
        }
    }
}
