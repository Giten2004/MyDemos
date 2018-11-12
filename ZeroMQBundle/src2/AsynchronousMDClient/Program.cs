using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace AsynchronousMDClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, ea) =>
            {
                ea.Cancel = true;
                cts.Cancel();
            };

            using (MajordomoClient session = new MajordomoClient("tcp://127.0.0.1:5555", Common.ProgramerHelper.Verbose))
            {
                int count;
                for (count = 0; count < 100000 && !cts.IsCancellationRequested; count++)
                {
                    ZMessage request = new ZMessage();
                    request.Prepend(new ZFrame("Hello world"));

                    session.Send("echo", request, cts);
                }
                for (count = 0; count < 100000 && !cts.IsCancellationRequested; count++)
                {
                    using (ZMessage reply = session.Recv(cts))
                        if (reply == null)
                            break; // Interrupt or failure
                }
                Console.WriteLine("{0} replies received\n", count);
            }
        }
    }
}
