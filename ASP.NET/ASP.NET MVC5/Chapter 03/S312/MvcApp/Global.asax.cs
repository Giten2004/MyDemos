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

            NinjectDependencyResolver dependencyResolver = new NinjectDependencyResolver();
            dependencyResolver.Register<IEmployeeRepository, EmployeeRepository>();
            DependencyResolver.SetResolver(dependencyResolver);
        }
    }
}
