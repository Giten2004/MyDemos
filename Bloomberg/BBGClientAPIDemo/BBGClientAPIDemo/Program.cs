using Bloomberglp.Blpapi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BBGClientAPIDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //RequestResponse reqRepDemo = new RequestResponse(new string[] { "CON7 COMDTY", "COQ7 COMDTY" }, new string[] { "LAST_TRADEABLE_DT" });
            //RequestResponse reqRepDemo = new RequestResponse(new string[] { "IBM US Equity" }, new string[] { "PX_LAST" });

            RequestResponse reqRepDemo = new RequestResponse(new string[] { "TLS AU Equity", "ARI AU Equity" }, new string[] { "DVD_EX_DT", "BDVD_NEXT_EST_EX_DT" });
            reqRepDemo.Run();

            Console.WriteLine("Press any key to exit");
            Console.Read();
        }
    }
}
