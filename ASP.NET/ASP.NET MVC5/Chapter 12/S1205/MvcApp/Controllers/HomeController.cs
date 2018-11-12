using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        [ValidateInput(false)]
        public void Action1(string foobar)
        {
            Response.Write(HttpUtility.HtmlEncode(foobar));
        }

        public void Action2(string foobar)
        {
            Response.Write(HttpUtility.HtmlEncode(foobar));
        }
    }
}