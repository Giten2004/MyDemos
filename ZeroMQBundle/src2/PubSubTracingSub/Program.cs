using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using ZeroMQ;

namespace PubSubTracingSub
{
    class Program
    {
        static void Main(string[] args)
        {
            // The subscriber thread requests messages starting with
            // A and B, then reads and counts incoming messages.
            using (var context = new ZContext())
            using (var subscriber = new ZSocket(context, ZSocketType.SUB))
            {
                subscriber.Connect("tcp://127.0.0.1:60001");
                subscriber.Subscribe("A");
                subscriber.Subscribe("B");

                ZError error;
                ZMessage msg;
                int count = 0;

                while (count < 5)
                {
                    if (null == (msg = subscriber.ReceiveMessage(out error)))
                    {
                        if (error == ZError.ETERM)
                            return; // Interrupted
                        throw new ZException(error);
                    }
                    msg.DumpZmsg("--------------");

                    ++count;
                }

                Console.WriteLine("I: subscriber counted {0}", count);
            }
        }
    }
}
