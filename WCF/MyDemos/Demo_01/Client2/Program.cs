using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace Client2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ChannelFactory<ICalculator> channelFactory = new ChannelFactory<ICalculator>(new WSHttpBinding(), "http://127.0.0.1:9999/calculatorservice"))
            {
                ICalculator proxy = channelFactory.CreateChannel();
                using (proxy as IDisposable)
                {
                    Console.WriteLine("x + y = {2} when x = {0} and y = {1}", 1, 2, proxy.Add(1, 2));
                    Console.WriteLine("x - y = {2} when x = {0} and y = {1}", 1, 2, proxy.Subtract(1, 2));
                    Console.WriteLine("x * y = {2} when x = {0} and y = {1}", 1, 2, proxy.Multiply(1, 2));
                    Console.WriteLine("x / y = {2} when x = {0} and y = {1}", 1, 2, proxy.Divide(1, 2));
                }
            }

            Console.ReadLine();
        }
    }
}
