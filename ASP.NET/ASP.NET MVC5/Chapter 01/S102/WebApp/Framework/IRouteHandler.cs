using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApp
{
    public interface IRouteHandler
    {
        IHttpHandler GetHttpHandler(RequestContext requestContext);
    }
}
