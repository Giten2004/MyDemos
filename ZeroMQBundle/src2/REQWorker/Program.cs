using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;
using Common;

namespace REQWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException();

            var workerName = args[0];
            var brokerBackendAddress = args[1];

            var context = ZContext.Create();

            // The worker task plugs into the load-balancer using a REQ socket

            using (var worker = new ZSocket(context, ZSocketType.REQ))
            {
                // Set printable identity
                worker.IdentityString = workerName;

                Console.WriteLine("Worker {0} connecting to broker backend Address: {1}", workerName, brokerBackendAddress);
                // Connect
                worker.Connect(brokerBackendAddress);

                // Tell broker we're ready for work
                worker.Send(new ZFrame("READY"));
                Console.WriteLine("sent: READY");

                // Process messages as they arrive
                ZError error;

                while (true)
                {
                    // Receive
                    var incomingMsg = worker.ReceiveMessage(out error);

                    if (incomingMsg == null)
                    {
                        if (error == ZError.ETERM)
                            return; // Interrupted

                        throw new ZException(error);
                    }

                    incomingMsg.DumpZmsg("---------received from broker--------------");

                    ZFrame incoming = incomingMsg[0];

                    using (incoming)
                    {
                        var task_id = incoming.ReadString();
                        Console.WriteLine("Worker {0}: {1}", workerName, task_id);

                        // Do some heavy work
                        Thread.Sleep(1);

                        // Send
                        worker.Send(new ZFrame(task_id));

                        Console.WriteLine("sent: {0}", task_id);
                    }
                }
            }
        }
    }
}
