using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client2
{
    class CalculateCallback : ICallback
    { 
        public void DisplayResult(double x, double y, double result)
         {
            Console.WriteLine("x + y = {2} when x = {0} and y = {1}", x, y, result);
        }
    }
}
