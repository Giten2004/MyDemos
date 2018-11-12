using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(this.GetControllerDescriptors(new FooController(), new BarController()));
        }

        private IEnumerable<ControllerDescriptor> GetControllerDescriptors(params Controller[] controllers)
        {
            controllers = controllers ?? new Controller[0];
            foreach (Controller controller in controllers)
            {
                ControllerContext.Controller = controller;
                SyncActionInvoker syncActionInvoker = controller.ActionInvoker as SyncActionInvoker;
                AsyncActionInvoker asyncActionInvoker = controller.ActionInvoker as AsyncActionInvoker;
                if (null != syncActionInvoker)
                {
                    yield return syncActionInvoker.GetControllerDescriptor(ControllerContext);
                }

                if (null != asyncActionInvoker)
                {
                    yield return asyncActionInvoker.GetControllerDescriptor(ControllerContext);
                }
            }
        }
    }

    public class FooController : Controller
    {
        protected override IActionInvoker CreateActionInvoker()
        {
            return new SyncActionInvoker();
        }
    }

    public class BarController : Controller
    {
        protected override IActionInvoker CreateActionInvoker()
        {
            return new AsyncActionInvoker();
        }
    }
}