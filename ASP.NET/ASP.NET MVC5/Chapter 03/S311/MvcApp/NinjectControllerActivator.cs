using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp
{
    public class NinjectControllerActivator : IControllerActivator
    {
        public IKernel Kernel { get; private set; }

        public NinjectControllerActivator()
        {
            this.Kernel = new StandardKernel();
        }

        public IController Create(RequestContext requestContext, Type controllerType)
        {
            return (IController)this.Kernel.TryGet(controllerType);
        }

        public void Register<TFrom, TTo>() where TTo : TFrom
        {
            this.Kernel.Bind<TFrom>().To<TTo>();
        }
    }
}