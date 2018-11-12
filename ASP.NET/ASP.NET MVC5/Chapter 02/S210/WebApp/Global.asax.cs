using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Mvc;

namespace WebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            object defaults = new
            {
                areacode = "010",
                days = 2,
                defaultCity = "BeiJing",
                defaultDays = 2,
                controller = "Home"
            };
            object constraints = new { areacode = @"0\d{2,3}", days = @"[1-3]" };
            string[] namespaces = new string[] { "Artech.Web.Mvc", "Artech.Web.Mvc.Html" };
            RouteTable.Routes.MapRoute("default", "{areacode}/{days}",defaults, constraints, namespaces);
        }
    }
}