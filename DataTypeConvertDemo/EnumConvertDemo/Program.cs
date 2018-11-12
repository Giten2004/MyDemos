using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumConvertDemo
{
    public enum Colors
    {
        None = -1,
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3
    }

    class Program
    {
        static void Main(string[] args)
        {
            //unsafe convert
            Colors colorOne = (Colors)200;
            colorOne = (Colors)Enum.ToObject(typeof(Colors), 200);

            //safe convert
            var safeColor = (Colors)Enum.Parse(typeof(Colors), "Red");
            safeColor = (Colors)Enum.Parse(typeof(Colors), "TT");

            var safeCorlorTwo = EnumUtil<Colors>.Parse(200);


            Console.ReadLine();
        }
    }
}
