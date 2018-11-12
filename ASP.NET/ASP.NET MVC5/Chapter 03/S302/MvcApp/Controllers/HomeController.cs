using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public new HttpResponse Response
        {
            get { return System.Web.HttpContext.Current.Response; }
        }

        protected override void Execute(RequestContext requestContext)
        {
            Response.Write("Execute(); <br/>");
            base.Execute(requestContext);
        }

        protected override void ExecuteCore()
        {
            Response.Write("ExecuteCore(); <br/>");
            base.ExecuteCore();
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext,AsyncCallback callback, object state)
        {
            Response.Write("BeginExecute(); <br/>");
            return base.BeginExecute(requestContext, callback, state);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            Response.Write("EndExecute(); <br/>");
            base.EndExecute(asyncResult);
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback,object state)
        {
            Response.Write("BeginExecuteCore(); <br/>");
            return base.BeginExecuteCore(callback, state);
        }

        protected override void EndExecuteCore(IAsyncResult asyncResult)
        {
            Response.Write("EndExecuteCore(); <br/>");
            base.EndExecuteCore(asyncResult);
        }

        protected override bool DisableAsyncSupport
        {
            get
            {
                return true;
            }
        }

        public ActionResult Index()
        {
            return Content("Index();<br/>");
        }
    }
}