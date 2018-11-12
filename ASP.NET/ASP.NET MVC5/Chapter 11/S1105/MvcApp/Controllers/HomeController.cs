using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Show(string id)
        {
            return View(id);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}