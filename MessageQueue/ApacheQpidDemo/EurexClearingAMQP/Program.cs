using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.Apache.Qpid.Messaging;
using Org.Apache.Qpid.Messaging.SessionReceiver;

namespace EurexClearingAMQP
{
    class Program
    {
        private static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            //CertificateTest test = new CertificateTest();
            //test.SubTest();

            log4net.Config.XmlConfigurator.Configure();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _Log.Debug("Begin to test");

            //EurexSimple eurexSimple = new EurexSimple();
            //eurexSimple.ConnectAndReceive();
            EurexProduct eurexProduct = new EurexProduct();
            eurexProduct.ConnectAndReceive();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
            _Log.Fatal(e.ToString());

            Console.ReadLine();
        }
    }
}
