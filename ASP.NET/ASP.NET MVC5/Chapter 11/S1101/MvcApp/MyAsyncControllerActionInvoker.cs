using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace MvcApp
{
    public class MyAsyncControllerActionInvoker : AsyncControllerActionInvoker
    {
        public new ControllerDescriptor GetControllerDescriptor(ControllerContext controllerContext)
        {
            return base.GetControllerDescriptor(controllerContext);
        }

        public new ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {
            return base.InvokeActionMethod(controllerContext, actionDescriptor, parameters);
        }
    }
}