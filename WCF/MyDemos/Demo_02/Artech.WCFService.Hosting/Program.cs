using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Artech.WCFService.Contract;
using Artech.WCFService.Service;
using System.ServiceModel.Description;

namespace Artech.WCFService.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            //HostCalculatorServiceViaCode();
            HostCalculatorSerivceViaConfiguration();
        }

        /// <summary>
        /// Hosting a service using managed code without any configuraiton information. 
        /// Please note that the related configuration data should be removed before calling the method.
        /// </summary>
        private static void HostCalculatorServiceViaCode()
        {
            var httpBaseAddress = new Uri("http://localhost:8888/generalCalculator");
            var tcpBaseAddress = new Uri("net.tcp://localhost:9999/generalCalculator");

            using (var calculatorSerivceHost = new ServiceHost(typeof(GeneralCalculatorService), httpBaseAddress, tcpBaseAddress))
            {
                var httpBinding = new BasicHttpBinding();
                var tcpBinding = new NetTcpBinding();

                calculatorSerivceHost.AddServiceEndpoint(typeof(IGeneralCalculator), httpBinding, string.Empty);
                calculatorSerivceHost.AddServiceEndpoint(typeof(IGeneralCalculator), tcpBinding, string.Empty);

                var behavior = calculatorSerivceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                {
                    if (behavior == null)
                    {
                        behavior = new ServiceMetadataBehavior();
                        behavior.HttpGetEnabled = true;
                        calculatorSerivceHost.Description.Behaviors.Add(behavior);
                    }
                    else
                    {
                        behavior.HttpGetEnabled = true;
                    }
                }

                calculatorSerivceHost.Opened +=
                    delegate { Console.WriteLine("Calculator Service has begun to listen ... ..."); };

                calculatorSerivceHost.Open();

                Console.Read();
            }
        }

        private static void HostCalculatorSerivceViaConfiguration()
        {
            using (var calculatorSerivceHost = new ServiceHost(typeof(GeneralCalculatorService)))
            {
                calculatorSerivceHost.Opened +=
                    delegate { Console.WriteLine("Calculator Service has begun to listen ... ..."); };

                calculatorSerivceHost.Open();

                Console.Read();
            }
        }
    }
}
