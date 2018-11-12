using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VM.Mvc.Extensions
{
    [AttributeUsage( AttributeTargets.Method)]
    public class HandleErrorActionAttribute: Attribute
    {
        public string HandleErrorAction { get; private set; }
        public HandleErrorActionAttribute(string handleErrorAction)
        {
            this.HandleErrorAction = handleErrorAction;
        }
    }
}