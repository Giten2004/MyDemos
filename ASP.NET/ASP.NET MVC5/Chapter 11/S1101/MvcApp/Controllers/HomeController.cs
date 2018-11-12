using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Foo()
        {
            return new RedirectResult("http://www.asp.net");
        }

        public void Bar()
        { }

        public ActionResult Baz()
        {
            return null;
        }

        public double Qux()
        {
            return 1.00;
        }

        public ActionResult Index()
        {
            MyAsyncControllerActionInvoker actionInvoker = new MyAsyncControllerActionInvoker();
            Dictionary<ActionDescriptor, ActionResult> actionResults = new Dictionary<ActionDescriptor, ActionResult>();
            ControllerDescriptor controllerDescriptor = actionInvoker.GetControllerDescriptor(this.ControllerContext);
            string[] actions = new string[] { "Foo", "Bar", "Baz", "Qux" };
            Array.ForEach(actions, action =>
            {
                ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(ControllerContext, action);
                ActionResult actionResult = actionInvoker.InvokeActionMethod(this.ControllerContext, actionDescriptor, new Dictionary<string, object>());
                actionResults.Add(actionDescriptor, actionResult);
            });
            return View(actionResults);
        }
    }
}