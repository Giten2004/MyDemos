using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Artech.WCFService.Client
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                //InvocateCalclatorServiceViaCode();

                InvocateCalclatorServiceViaConfiguration();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.Read();
        }

        private static void InvocateCalclatorServiceViaCode()
        {
            Binding httpBinding = new BasicHttpBinding();
            Binding tcpBinding = new NetTcpBinding();

            var httpAddress = new EndpointAddress("http://localhost:8888/generalCalculator");
            var tcpAddress = new EndpointAddress("net.tcp://localhost:9999/generalCalculator");
            var httpAddress_iisHost = new EndpointAddress("http://localhost/wcfservice/GeneralCalculatorService.svc");

            Console.WriteLine("Invocate self-host calculator service... ...");

            #region Invocate Self-host service

            using (var calculator_http = new GeneralCalculatorClient(httpBinding, httpAddress))
            {
                using (var calculator_tcp = new GeneralCalculatorClient(tcpBinding, tcpAddress))
                {
                    try
                    {
                        Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_http.Add(1, 2));

                        Console.WriteLine("Begin to invocate calculator service via tcp transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_tcp.Add(1, 2));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            #endregion

            Console.WriteLine("\n\nInvocate IIS-host calculator service... ...");

            #region Invocate IIS-host service

            using (var calculator = new GeneralCalculatorClient(httpBinding, httpAddress_iisHost))
            {
                try
                {
                    Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                    Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator.Add(1, 2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            #endregion
        }

        private static void InvocateCalclatorServiceViaConfiguration()
        {
            Console.WriteLine("Invocate self-host calculator service... ...");

            #region Invocate Self-host service

            using (var calculator_http = new GeneralCalculatorClient("selfHostEndpoint_http"))
            {
                using (var calculator_tcp = new GeneralCalculatorClient("selfHostEndpoint_tcp"))
                {
                    try
                    {
                        Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_http.Add(1, 2));

                        Console.WriteLine("Begin to invocate calculator service via tcp transport... ...");
                        Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator_tcp.Add(1, 2));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            #endregion

            Console.WriteLine("\n\nInvocate IIS-host calculator service... ...");

            #region Invocate IIS-host service

            using (var calculator = new GeneralCalculatorClient("iisHostEndpoint"))
            {
                try
                {
                    Console.WriteLine("Begin to invocate calculator service via http transport... ...");
                    Console.WriteLine("x + y = {2} where x = {0} and y = {1}", 1, 2, calculator.Add(1, 2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            #endregion
        }
    }
}
