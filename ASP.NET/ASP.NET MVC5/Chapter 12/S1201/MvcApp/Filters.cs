using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public abstract class FilterBaseAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) { }
        public void OnActionExecuting(ActionExecutingContext filterContext) { }
    }

    public class FooAttribute : FilterBaseAttribute { }
    public class BarAttribute : FilterBaseAttribute { }
    public class BazAttribute : FilterBaseAttribute { } 

}