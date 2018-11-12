using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public partial class Default : System.Web.UI.Page
    {
        private RouteData routeData;
        public RouteData GetRouteData()
        {
            if (null != routeData)
            {
                return routeData;
            }
            HttpRequest request = new HttpRequest("default.aspx", "http://localhost/0512/3", null);
            HttpResponse response = new HttpResponse(new StringWriter());
            HttpContext context = new HttpContext(request, response);
            HttpContextBase contextWrapper = new HttpContextWrapper(context);
            return routeData = RouteTable.Routes.GetRouteData(contextWrapper);
        }
    }
}