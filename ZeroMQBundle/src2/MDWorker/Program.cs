using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace MDWorker
{
    class Program
    {
        //  Majordomo Protocol worker example
        //  Uses the mdwrk API to hide all MDP aspects
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, ea) =>
            {
                ea.Cancel = true;
                cts.Cancel();
            };

            using (MajordomoWorker session = new MajordomoWorker("tcp://127.0.0.1:5555", "echo", Common.ProgramerHelper.Verbose))
            {
                ZMessage reply = null;
                while (true)
                {
                    ZMessage request = session.Recv(reply, cts);
                    if (request == null)
                        break; // worker was interrupted
                    reply = request; // Echo is complex
                }
            }
        }
    }
}
