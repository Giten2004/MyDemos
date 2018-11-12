using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class RouteTable
    {
        public static RouteDictionary Routes { get; private set; }
        static RouteTable()
        {
            Routes = new RouteDictionary();
        }
    }

}