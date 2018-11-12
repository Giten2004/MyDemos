using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public void Index(string id)
        {
            foreach (var variable in RouteData.Values)
            {
                Response.Write(string.Format("{0}: {1}<br/>",variable.Key, variable.Value));
            }
        }
    }
}