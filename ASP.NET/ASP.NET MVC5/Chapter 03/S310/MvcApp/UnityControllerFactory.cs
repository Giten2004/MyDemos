using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        public IUnityContainer UnityContainer { get; private set; }

        public UnityControllerFactory(IUnityContainer unityContainer)
        {
            this.UnityContainer = unityContainer;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (null == controllerType)
            {
                return null;
            }
            return (IController)this.UnityContainer.Resolve(controllerType);
        }
    }
}