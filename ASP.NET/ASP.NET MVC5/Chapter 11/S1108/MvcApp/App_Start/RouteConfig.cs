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
            name: "Default",
            url: "{culture}/{controller}/{action}",
            defaults: new
            {
                culture = "zh-CN",
                controller = "Home",
                action = "Index"
            }
        );

        }
    }
}
