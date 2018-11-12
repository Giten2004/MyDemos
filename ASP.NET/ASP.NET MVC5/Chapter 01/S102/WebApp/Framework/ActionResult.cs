using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp
{
    public abstract class ActionResult
    {
        public abstract void ExecuteResult(ControllerContext context);
    }
}
