using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public abstract class FilterBaseAttribute : FilterAttribute, IActionFilter
    {
        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Write(string.Format("{0}.OnActionExecuted()<br/>", this.GetType().Name));
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write(string.Format("{0}.OnActionExecuting()<br/>", this.GetType().Name));
        }
    }

    public class FooAttribute : FilterBaseAttribute { }
    public class BarAttribute : FilterBaseAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.Result = new EmptyResult();
        }
    }
    public class BazAttribute : FilterBaseAttribute { }
}