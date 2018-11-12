using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new Person());
        }

        [HttpPost]
        public ActionResult Index(Person person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }
            else
            {
                return Content("输入数据通过验证");
            }
        }
    }
}