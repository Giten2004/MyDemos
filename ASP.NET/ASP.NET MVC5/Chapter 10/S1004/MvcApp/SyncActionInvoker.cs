using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class SyncActionInvoker : IActionInvoker
    {
        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            throw new NotImplementedException();
        }
    }
}