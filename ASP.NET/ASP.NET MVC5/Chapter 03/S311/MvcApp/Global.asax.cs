using System.Web.Mvc;
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

            NinjectControllerActivator controllerActivator = new NinjectControllerActivator();
            controllerActivator.Register<IEmployeeRepository, EmployeeRepository>();
            DefaultControllerFactory controllerFactory = new DefaultControllerFactory(controllerActivator);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}
