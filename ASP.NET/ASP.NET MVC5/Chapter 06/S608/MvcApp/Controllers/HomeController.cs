using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public void DemoAction([ModelBinder(typeof(BazModelBinder))]Foo foo, Bar bar, Baz baz)
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