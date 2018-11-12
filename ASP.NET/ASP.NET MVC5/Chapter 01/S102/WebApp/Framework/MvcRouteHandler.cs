using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApp
{
    public class MvcRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MvcHandler(requestContext);
        }
    }
}
