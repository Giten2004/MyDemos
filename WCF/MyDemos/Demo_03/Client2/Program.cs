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
            InstanceContext instanceContext = new InstanceContext(new CalculateCallback());

            using (DuplexChannelFactory<ICalculator> channelFactory = new DuplexChannelFactory<ICalculator>(instanceContext, "CalculatorService"))
            {
                ICalculator proxy = channelFactory.CreateChannel();

                using (proxy as IDisposable)
                {
                    proxy.Add(1, 2);

                    Console.Read();
                }
            }
        }
    }
}