using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApp
{
    public class MvcHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }
        public RequestContext RequestContext { get; private set; }

        public MvcHandler(RequestContext requestContext)
        {
            this.RequestContext = requestContext;
        }

        public void ProcessRequest(HttpContext context)
        {
            string controllerName = this.RequestContext.RouteData.Controller;
            IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            IController controller = controllerFactory.CreateController(this.RequestContext, controllerName);
            controller.Execute(this.RequestContext);
        }
    }
}
