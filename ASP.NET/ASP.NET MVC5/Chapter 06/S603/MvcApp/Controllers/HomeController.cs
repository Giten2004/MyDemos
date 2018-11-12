using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(); 
        }

        public ActionResult DataOfChildActionValueProvider()
        {
            ControllerContext.RouteData.Values["Foo"] = "abc";
            ControllerContext.RouteData.Values["Bar"] = "ijk";
            ControllerContext.RouteData.Values["Baz"] = "xyz";

            ChildActionValueProvider valueProvider =new ChildActionValueProvider(ControllerContext);
            return View(valueProvider);
        }
    }
}