using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ.lib;

namespace ReportVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            int major, minor, patch;

            zmq.version(out major, out minor, out patch);

            Console.WriteLine("ZeroMQLib version: {0}.{1}.{2}", major, minor, patch);

            Console.ReadLine();
        }
    }
}
