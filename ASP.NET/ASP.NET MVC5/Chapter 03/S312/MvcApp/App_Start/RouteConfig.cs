using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Home",
               url: "",
               defaults: new { controller = "Employees", action = "GetAllEmployees" }
            );

            routes.MapRoute(
               name: "Detail",
               url: "{name}/{id}",
               defaults: new { controller = "Employees", action = "GetEmployeeById" }
            );
        }
    }
}
