using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ReflectedControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(typeof(HomeController));
            return View(controllerDescriptor.GetCanonicalActions());
        }

        public void Action1()
        { }

        public static void Action2()
        { }

        private void Action3()
        { }

        public void Action4<T>()
        { }

        public void Action5(ref object obj)
        { }

        public void Action6(out object obj)
        {
            obj = null;
        }
    }
}