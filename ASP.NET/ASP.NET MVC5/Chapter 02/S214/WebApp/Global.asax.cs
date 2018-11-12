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
            UriTemplateRoute route = new UriTemplateRoute("{areacode=010}/{days=2}","~/Weather.aspx", new { defualtCity = "BeiJing", defaultDays = 2 });
            RouteTable.Routes.Add("default", route);
        }
    }
}