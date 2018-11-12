using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public string Index(string id)
        {
            return string.Format("Hello, {0}", id);
        }

        protected override IActionInvoker CreateActionInvoker()
        {
            return new MyControllerActionInvoker();
        }
    }
}