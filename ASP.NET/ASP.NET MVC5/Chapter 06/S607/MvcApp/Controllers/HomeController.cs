using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApp;
using System.Data.Linq;
using System.Threading;
using System.Reflection;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public void DemoAction(Foo foo, Bar bar, Baz baz)
        {
        }

        public ActionResult Index()
        {
            ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(typeof(HomeController));
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(ControllerContext, "DemoAction");
            Dictionary<ParameterDescriptor, IModelBinder> binders = new Dictionary<ParameterDescriptor, IModelBinder>();
            foreach (ParameterDescriptor parameterDescriptor in actionDescriptor.GetParameters())
            {
                binders.Add(parameterDescriptor, this.GetModelBinder(parameterDescriptor));
            }
            return View(binders);
        }

        private IModelBinder GetModelBinder(ParameterDescriptor parameterDescriptor)
        {
            MethodInfo getModelBinder = typeof(ControllerActionInvoker).GetMethod("GetModelBinder", BindingFlags.Instance | BindingFlags.NonPublic);
            return (IModelBinder)getModelBinder.Invoke(this.ActionInvoker, new object[] { parameterDescriptor });
        }
    }
}