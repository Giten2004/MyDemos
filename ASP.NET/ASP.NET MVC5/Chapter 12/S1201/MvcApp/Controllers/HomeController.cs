using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    [Foo]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ReflectedControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(typeof(HomeController));
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(ControllerContext, "DemoAction");
            IEnumerable<Filter> filters = FilterProviders.Providers.GetFilters(ControllerContext, actionDescriptor);
            return View(filters);
        }

        [Bar]
        public void DemoAction()
        { }
    }
}