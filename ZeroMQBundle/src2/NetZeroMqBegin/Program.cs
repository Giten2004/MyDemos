using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace NetZeroMqBegin
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var server = new ResponseSocket("@tcp://localhost:5556")) // bind
            //using (var client = new RequestSocket(">tcp://localhost:5556"))  // connect
            //{
            //    // Send a message from the client socket
            //    client.SendFrame("Hello");

            //    // Receive the message from the server socket
            //    string m1 = server.ReceiveFrameString();
            //    Console.WriteLine("From Client: {0}", m1);

            //    // Send a response back from the server
            //    server.SendFrame("Hi Back");

            //    // Receive the response from the client socket
            //    string m2 = client.ReceiveFrameString();
            //    Console.WriteLine("From Server: {0}", m2);
            //}

            const int MegaBit = 1024;
            const int MegaByte = 1024;

            using (var pub = new PublisherSocket())
            using (var sub1 = new SubscriberSocket())
            using (var sub2 = new SubscriberSocket())
            {
                pub.Options.MulticastHops = 2;
                pub.Options.MulticastRate = 40 * MegaBit; // 40 megabit
                pub.Options.MulticastRecoveryInterval = TimeSpan.FromMinutes(10);
                pub.Options.SendBuffer = MegaByte * 10; // 10 megabyte
                pub.Connect("pgm://224.0.0.1:5555");

                sub1.Options.ReceiveBuffer = MegaByte * 10;
                sub1.Bind("pgm://224.0.0.1:5555");
                sub1.Subscribe("");

                sub2.Bind("pgm://224.0.0.1:5555");
                sub2.Options.ReceiveBuffer = MegaByte * 10;
                sub2.Subscribe("");

                Console.WriteLine("Server sending 'Hi'");
                pub.Send("Hi");

                bool more;
                Console.WriteLine("sub1 received = '{0}'", sub1.ReceiveString(out more));
                Console.WriteLine("sub2 received = '{0}'", sub2.ReceiveString(out more));
            }


            Console.ReadLine();
        }
    }
}
