using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Routing;

namespace MvcApp
{
    public class CultureRouteConstraint : IRouteConstraint
    {
        private static IList<string> allCultures;
        static CultureRouteConstraint()
        {
            allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(culture => culture.Name).ToList();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            object culture;
            if (values.TryGetValue(parameterName, out culture))
            {
                return allCultures.Any(c => string.Compare(c, culture.ToString(),true) == 0);
            }
            return false;
        }
    }
}