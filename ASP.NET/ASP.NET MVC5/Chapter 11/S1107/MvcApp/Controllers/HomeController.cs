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
        public void Index()
        {
            ViewData.Model = new Contact
            {
                Name = "张三",
                PhoneNo = "123456789",
                EmailAddress = "zhangsan@gmail.com"
            };
            SimpleRazorView view = new SimpleRazorView("~/Views/Home/Index.cshtml");
            ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, Response.Output);
            view.Render(viewContext, viewContext.Writer);
        }
    }
}