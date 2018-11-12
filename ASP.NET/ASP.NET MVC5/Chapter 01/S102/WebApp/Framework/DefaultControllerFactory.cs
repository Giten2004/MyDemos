using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Compilation;

namespace WebApp
{
    public class DefaultControllerFactory : IControllerFactory
    {
        private static List<Type> controllerTypes = new List<Type>();

        static DefaultControllerFactory()
        {
            foreach (Assembly assembly in BuildManager.GetReferencedAssemblies())
            {
                foreach (Type type in assembly.GetTypes().Where(type => typeof(IController).IsAssignableFrom(type)))
                {
                    controllerTypes.Add(type);
                }
            }
        }

        public IController CreateController(RequestContext requestContext,string controllerName)
        {
            string typeName = controllerName + "Controller";
            Type controllerType = controllerTypes.FirstOrDefault(c => string.Compare(typeName, c.Name, true) == 0);
            if (null == controllerType)
            {
                return null;
            }
            return (IController)Activator.CreateInstance(controllerType);
        }
    }
}
