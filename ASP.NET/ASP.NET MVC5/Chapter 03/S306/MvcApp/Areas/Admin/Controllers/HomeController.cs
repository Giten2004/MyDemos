using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Areas.Controllers
{
    public class HomeController : Controller
    {
        public void Index()
        {
            Response.Write(string.Format("UseNamespaceFallback: {0}<br/>",RouteData.DataTokens["UseNamespaceFallback"]));
            Response.Write(string.Format("Controller Type: {0}<br/>",this.GetType().FullName));
        }
    }
}
