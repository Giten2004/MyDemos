using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using ZeroMQ;

namespace PubSubTracing
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Espresso Pattern
            // This shows how to capture data using a pub-sub proxy
            //
            // Author: metadings
            //

            using (var context = new ZContext())
            using (var frontend = new ZSocket(context, ZSocketType.XSUB))
            using (var backend = new ZSocket(context, ZSocketType.XPUB))
            using (var listener = new ZSocket(context, ZSocketType.PAIR))
            {
                //new Thread(() => Espresso_Publisher(context)).Start();
                //new Thread(() => Espresso_Subscriber(context)).Start();
                new Thread(() => Espresso_Listener(context)).Start();

                frontend.Connect("tcp://127.0.0.1:60000");
                backend.Bind("tcp://*:60001");

                listener.Bind("inproc://listener");

                ZError error;
                if (!ZContext.Proxy(frontend, backend, listener, out error))
                {
                    if (error == ZError.ETERM)
                        return; // Interrupted
                    throw new ZException(error);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        static void Espresso_Publisher(ZContext context)
        {
            
        }

        static void Espresso_Subscriber(ZContext context)
        {
        }

        static void Espresso_Listener(ZContext context)
        {
            // The listener receives all messages flowing through the proxy, on its
            // pipe. In CZMQ, the pipe is a pair of ZMQ_PAIR sockets that connect
            // attached child threads. In other languages your mileage may vary:

            using (var listener = new ZSocket(context, ZSocketType.PAIR))
            {
                listener.Connect("inproc://listener");

                ZError error;
                ZFrame frame;

                while (true)
                {
                    if (null != (frame = listener.ReceiveFrame(out error)))
                    {
                        using (frame)
                        {
                            byte first = frame.ReadAsByte();

                            var rest = new byte[9];
                            frame.Read(rest, 0, rest.Length);

                            Console.WriteLine("{0} {1}", (char)first, rest.ToHexString());

                            if (first == 0x01)
                            {
                                // Subscribe
                            }
                            else if (first == 0x00)
                            {
                                // Unsubscribe
                                //context.Shutdown();
                            }
                        }
                    }
                    else
                    {
                        if (error == ZError.ETERM)
                            return; // Interrupted
                        throw new ZException(error);
                    }
                }
            }
        }
    }
}