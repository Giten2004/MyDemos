using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amqpnetliteDemo
{
    class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static void Main(string[] args)
        {
            var log4netConfigStream = File.Open(@"log4net.config", FileMode.Open);
            log4net.Config.XmlConfigurator.Configure(log4netConfigStream);

            Log.Debug("log4net inited.");

            var broadcastReceiver = new BroadcastReceiver();
            broadcastReceiver.Start();

            Console.ReadKey();
        }
    }
}
