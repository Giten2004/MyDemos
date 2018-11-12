using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp.Controllers
{
    public class HomeController : IController
    {
        public void ValidAction()
        {
        }

        public void Execute(RequestContext requestContext)
        {
            this.ListValidActions(requestContext);
        }

        private void ListValidActions(RequestContext requestContext)
        {
            ReflectedControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(typeof(HomeController));
            ActionDescriptor[] actionDescriptors = controllerDescriptor.GetCanonicalActions();
            if (actionDescriptors.Any())
            {
                foreach (ActionDescriptor actionDescriptor in actionDescriptors)
                {
                    requestContext.HttpContext.Response.Write(actionDescriptor.ActionName + "<br/>");
                }
            }
            else
            {
                requestContext.HttpContext.Response.Write("无合法的Action方法");
            }
        }
    }
}