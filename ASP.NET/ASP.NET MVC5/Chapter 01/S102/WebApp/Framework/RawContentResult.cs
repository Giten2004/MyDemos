using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebApp
{
    public class RawContentResult : ActionResult
    {
        public Action<TextWriter> Callback { get; private set; }
        public RawContentResult(Action<TextWriter> action)
        {
            this.Callback = action;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            this.Callback(context.RequestContext.HttpContext.Response.Output);
        }
    }
}
