using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using Common;

namespace MDClient
{
    class Program
    {
        //  Majordomo Protocol client example
        //  Uses the mdcli API to hide all MDP aspects
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, ea) =>
            {
                ea.Cancel = true;
                cts.Cancel();
            };

            using (MajordomoClient session = new MajordomoClient("tcp://127.0.0.1:5555", ProgramerHelper.Verbose))
            {
                int count;
                for (count = 0; count < 100000; count++)
                {
                    ZMessage request = new ZMessage();
                    request.Prepend(new ZFrame("Hello world"));

                    using (ZMessage reply = session.Send("echo", request, cts))
                        if (reply == null)
                            break; // Interrupt or failure
                }
                Console.WriteLine("{0} requests/replies processed\n", count);
            }
        }
    }
}