using MvcApp.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new Contact());
        }

        [HttpPost]
        public ActionResult Index(Contact contact)
        {
            return View(contact);
        }
    }
}