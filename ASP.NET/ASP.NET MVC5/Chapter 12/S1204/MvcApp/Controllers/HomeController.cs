using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    [Authenticate]
    public class HomeController : Controller
    {
        public void Index()
        {
            Response.Write(string.Format("Controller.User： {0}<br/>", this.User.Identity.Name));
            Response.Write(string.Format("HttpContext.User： {0}<br/>", this.ControllerContext.HttpContext.User.Identity.Name));
            Response.Write(string.Format("Thread.CurrentPrincipal： {0}<br/>", Thread.CurrentPrincipal.Identity.Name));
        }
    }
}