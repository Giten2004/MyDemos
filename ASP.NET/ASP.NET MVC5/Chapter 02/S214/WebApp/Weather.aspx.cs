using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public partial class Weather : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {}

        public string GenerateUrl(string areacode, int days)
        {
            var values = new { areacode = areacode, days = days };
            RequestContext requestContext = new RequestContext();
            requestContext.HttpContext = new HttpContextWrapper(HttpContext.Current);
            requestContext.RouteData = RouteData;
            return RouteTable.Routes.GetVirtualPath(requestContext, new RouteValueDictionary(values)).VirtualPath;
        }
    }
}