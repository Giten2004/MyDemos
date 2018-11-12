using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class FooController : Controller
    {
        public ActionResult Action1()
        {
            return View();
        }
        public ActionResult Action2()
        {
            return View();
        }
    }

    public class BarController : Controller
    {
        public ActionResult Action1()
        {
            return View();
        }
        public ActionResult Action2()
        {
            return View();
        }
    }
}