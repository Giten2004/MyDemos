using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        [Foo(Order = 1)]
        [Bar(Order = 2)]
        [Baz(Order = 3)]
        public void Index()
        {
            Response.Write("Index...</br>");
        }
    }
}