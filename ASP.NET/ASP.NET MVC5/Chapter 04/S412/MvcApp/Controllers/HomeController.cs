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
            Employee employee = new Employee
            {
                Name = "张三",
                Gender = "M",//男
                Education = "M",//硕士
                Departments = new string[] { "DEV01", "DEV02" },
                Skills = new string[] { "CSharp", "AdoNet" }//C#，ADO.NET
            };
            return View(employee);
        }
    }
}