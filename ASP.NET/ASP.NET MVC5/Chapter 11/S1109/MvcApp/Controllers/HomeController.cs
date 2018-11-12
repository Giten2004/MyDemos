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
        public ActionResult Index()
        {
            Contact contact = new Contact
            {
                Name = "张三",
                PhoneNo = "123456789",
                EmailAddress = "zhangsan@gmail.com"
            };
            return View(contact);
        }
    }
}