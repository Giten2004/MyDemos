using System.Collections.Generic;
using System.Data.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public void DemoAction(byte[] parameter1, Binary parameter2, HttpPostedFile parameter3, CancellationToken parameter4, string parameter5)
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