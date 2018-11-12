using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.CalculatorServices;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (CalculatorServiceClient proxy = new CalculatorServiceClient())
            {
                Console.WriteLine("x + y = {2} when x = {0} and y = {1}", 1, 2, proxy.Add(1, 2));
                Console.WriteLine("x - y = {2} when x = {0} and y = {1}", 1, 2, proxy.Subtract(1, 2));
                Console.WriteLine("x * y = {2} when x = {0} and y = {1}", 1, 2, proxy.Multiply(1, 2));
                Console.WriteLine("x / y = {2} when x = {0} and y = {1}", 1, 2, proxy.Divide(1, 2));
            }

            Console.ReadLine();
        }
    }
}
