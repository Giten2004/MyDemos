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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Image(string id)
        {
            string path = Server.MapPath("/images/" + id + ".jpg");
            return File(path, "image/jpeg");
        }
    }
}