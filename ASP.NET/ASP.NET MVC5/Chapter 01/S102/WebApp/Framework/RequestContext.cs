using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApp
{
    public class RequestContext
    {
        public virtual HttpContextBase HttpContext { get; set; }
        public virtual RouteData RouteData { get; set; }
    }
}
