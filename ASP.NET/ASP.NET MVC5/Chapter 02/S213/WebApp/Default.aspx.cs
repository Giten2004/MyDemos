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
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpRequest request = new HttpRequest("default.aspx", "http://localhost:3721/products/getproduct/001", null);
            HttpResponse response = new HttpResponse(new StringWriter());
            HttpContext context = new HttpContext(request, response);
            HttpContextBase contextWrapper = new HttpContextWrapper(context);

            RouteData routeData = RouteTable.Routes.GetRouteData(contextWrapper);
            RequestContext requestContext = new RequestContext(contextWrapper, routeData);
            RouteHelper helper = new RouteHelper(requestContext);

            Response.Write(helper.Action("GetProductCategories") + "<br/>");
            Response.Write(helper.Action("GetAllContacts", "Sales") + "<br/>");
            Response.Write(helper.Action("GetAllContact", "Sales",new { id = "001" }) + "<br/>");
            Response.Write(helper.Action("GetAllContact", "Sales",new { id = "001" }, "https") + "<br/>");
            Response.Write(helper.Action("GetAllContact", "Sales",new { id = "001" }, "https", "www.artech.com") + "<br/>");
        }
    }
}