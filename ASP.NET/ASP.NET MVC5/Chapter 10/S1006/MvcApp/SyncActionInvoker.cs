using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class SyncActionInvoker : ControllerActionInvoker
    {
        public new ControllerDescriptor GetControllerDescriptor(ControllerContext controllerContext)
        {
            return base.GetControllerDescriptor(controllerContext);
        }
    }
}