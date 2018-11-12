using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using ZeroMQ;

namespace PubSubTracingPub
{
    class Program
    {
        static void Main(string[] args)
        {
            // The publisher sends random messages starting with A-J:

            using (var context = new ZContext())
            using (var publisher = new ZSocket(context, ZSocketType.PUB))
            {
                publisher.Bind("tcp://*:60000");

                ZError error;

                while (true)
                {
                    var frame = ZFrame.Create(8);
                    var bytes = new byte[8];
                    using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
                    {
                        //用一个随机值填充数组。
                        rng.GetBytes(bytes);
                    }
                    Console.WriteLine("Frame Content: {0}", bytes.ToHexString());

                    frame.Write(bytes, 0, 8);

                    if (!publisher.SendFrame(frame, out error))
                    {
                        if (error == ZError.ETERM)
                            return; // Interrupted
                        throw new ZException(error);
                    }

                    Thread.Sleep(1);
                }
            }
        }
    }
}
