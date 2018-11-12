using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            int major, minor, patch;
            major = ZeroMQ.ZmqVersion.Current.Major;
            minor = ZeroMQ.ZmqVersion.Current.Minor;
            patch = ZeroMQ.ZmqVersion.Current.Patch;

            Console.WriteLine("ZeroMQLib version: {0}.{1}.{2}", major, minor, patch);

            Console.ReadLine();
        }
    }
}
