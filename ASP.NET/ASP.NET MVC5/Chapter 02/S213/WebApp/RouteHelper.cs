using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApp
{
    public class RouteHelper
    {
        public RequestContext RequestContext { get; private set; }
        public RouteCollection RouteCollection { get; private set; }
        public RouteHelper(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
            this.RouteCollection = RouteTable.Routes;
        }

        public string Action(string actionName, string controllerName = null, object routeValues = null, string protocol = null, string hostName = null)
        {
            controllerName = controllerName ??this.RequestContext.RouteData.GetRequiredString("controller");
            RouteValueDictionary routeValueDictionary = new RouteValueDictionary(routeValues);
            routeValueDictionary.Add("action", actionName);
            routeValueDictionary.Add("controller", controllerName);

            string virtualPath = this.RouteCollection.GetVirtualPath(this.RequestContext, routeValueDictionary).VirtualPath;

            if (string.IsNullOrEmpty(protocol) && string.IsNullOrEmpty(hostName))
            {
                return virtualPath.ToLower();
            }

            protocol = protocol ?? "http";
            Uri uri = this.RequestContext.HttpContext.Request.Url;
            hostName = hostName ?? uri.Host + ":" + uri.Port;
            return string.Format("{0}://{1}{2}", protocol, hostName, virtualPath).ToLower();
        }
    }
}