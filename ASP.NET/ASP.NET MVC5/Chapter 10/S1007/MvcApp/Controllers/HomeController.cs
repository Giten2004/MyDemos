using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace MvcApp.Controllers
{
    public class HomeController : AsyncController
    {
        protected override IActionInvoker CreateActionInvoker()
        {
            return new AsyncActionInvoker();
        }

        public ActionResult Index()
        {
            AsyncActionInvoker actionInvoker = (AsyncActionInvoker)this.ActionInvoker;
            ControllerDescriptor controllerDescriptor = actionInvoker.GetControllerDescriptor(ControllerContext);
            List<ActionDescriptor> actionDescriptors = new List<ActionDescriptor>();

            actionDescriptors.Add(controllerDescriptor.FindAction(ControllerContext, "Foo"));
            actionDescriptors.Add(controllerDescriptor.FindAction(ControllerContext, "Bar"));
            actionDescriptors.Add(controllerDescriptor.FindAction(ControllerContext, "Baz"));

            return View(actionDescriptors);
        }


        public void Foo() { }
        public void BarAsync() { }
        public void BarCompleted() { }
        public Task<ActionResult> Baz()
        {
            throw new NotImplementedException();
        }
    }
}