using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityContainer unityContainer = new UnityContainer();
            unityContainer.RegisterType<IEmployeeRepository, EmployeeRepository>();
            UnityControllerFactory controllerFactory =new UnityControllerFactory(unityContainer);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
