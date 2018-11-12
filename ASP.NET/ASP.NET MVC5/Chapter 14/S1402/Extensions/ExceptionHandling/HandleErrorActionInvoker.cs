using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VM.Mvc.Extensions
{
    public class HandleErrorActionInvoker : ControllerActionInvoker
    {
        public virtual ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            IDictionary<string, object> parameterValues = this.GetParameterValues(controllerContext, actionDescriptor);
            return base.InvokeActionMethod(controllerContext, actionDescriptor,parameterValues);
        }
    }
}