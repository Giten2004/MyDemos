using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class UrlRoutingModule : IHttpModule
    {
        public void Dispose()
        { }

        public void Init(HttpApplication context)
        {
            context.PostResolveRequestCache += OnPostResolveRequestCache;
        }
        protected virtual void OnPostResolveRequestCache(object sender, EventArgs e)
        {
            HttpContextWrapper httpContext = new HttpContextWrapper(HttpContext.Current);
            RouteData routeData = RouteTable.Routes.GetRouteData(httpContext);
            if (null == routeData)
            {
                return;
            }
            RequestContext requestContext = new RequestContext
            {
                RouteData = routeData,
                HttpContext = httpContext
            };
            IHttpHandler handler = routeData.RouteHandler.GetHttpHandler(requestContext);
            httpContext.RemapHandler(handler);
        }
    }
}