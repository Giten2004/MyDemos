using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

namespace MvcApp
{
public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

        DefaultInlineConstraintResolver constraintResolver = new DefaultInlineConstraintResolver();
        constraintResolver.ConstraintMap.Add("culture", typeof(CultureRouteConstraint));
        routes.MapMvcAttributeRoutes(constraintResolver);

        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
    }
}
}
