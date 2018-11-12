using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;
using Common;
using System.Threading;

namespace REQClientTask
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
                throw new ArgumentException();

            var clientName = args[0];
            var brokerAddress = args[1];
            var monitorAddress = args[2];

            var context = ZContext.Create();
            using (ZSocket reqSocket = new ZSocket(context, ZSocketType.REQ),
                   monitorSocket = new ZSocket(context, ZSocketType.PUSH))
            {
                Console.WriteLine("Client {0} connecting to broker {1}", clientName, brokerAddress);
                reqSocket.Connect(brokerAddress);

                Console.WriteLine("Client {0} connecting to monitor {1}", clientName, monitorAddress);
                reqSocket.Connect(monitorAddress);

                ZError error;

                var poll = ZPollItem.CreateReceiver();

                while (true)
                {
                    var ramdomSleep = new Random(5);
                    //Thread.Sleep(ramdomSleep.Next());

                    var burstRamdom = new Random(15);
                    var burst = burstRamdom.Next();
                    while (burst-- > 0)
                    {
                        var task_id = ramdomSleep.Next(0x10000).ToString("X4");
                        //  Send request with random hex ID
                        reqSocket.Send(new ZFrame(task_id));
                        Console.WriteLine("sent: {0}", task_id);

                        //  Wait max ten seconds for a reply, then complain
                        // Receive
                        ZMessage incomingMsg;
                        if (reqSocket.PollIn(poll, out incomingMsg, out error, new TimeSpan(0, 0, 10)))
                        {
                            //  Worker is supposed to answer us with our task id
                            incomingMsg.DumpZmsg("-----------Receivd From router---------");
                            ZFrame incomingFrame = incomingMsg[0];

                            using (incomingFrame)
                            {
                                var replyMsg = incomingFrame.ReadString();

                                Console.WriteLine("Client {0} received: {1}", clientName, replyMsg);

                                monitorSocket.Send(new ZFrame(replyMsg));
                            }
                        }

                        if (error == ZError.ETERM)
                        {
                            monitorSocket.Send(new ZFrame(string.Format("E: CLIENT EXIT - lost task {0}", task_id)));
                            return;
                        }
                    }
                }
            }
        }
    }
}
