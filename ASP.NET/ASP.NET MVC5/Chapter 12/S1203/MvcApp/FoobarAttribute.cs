using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,AllowMultiple = false)]
    public class FoobarAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }
        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }
}