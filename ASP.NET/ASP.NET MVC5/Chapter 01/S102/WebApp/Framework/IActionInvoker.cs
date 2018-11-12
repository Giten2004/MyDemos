using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp
{
    public interface IActionInvoker
    {
        void InvokeAction(ControllerContext controllerContext, string actionName);
    }
}
