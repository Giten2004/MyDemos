using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class MyReflectedActionDescriptor : ReflectedActionDescriptor
    {
        public ActionExecutor ActionExecutor { get; private set; }

        public MyReflectedActionDescriptor(ReflectedActionDescriptor actionDescriptor)
            : base(actionDescriptor.MethodInfo, actionDescriptor.ActionName, actionDescriptor.ControllerDescriptor)
        {
            this.ActionExecutor = new ActionExecutor(actionDescriptor.MethodInfo);
        }
        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            List<object> arguments = new List<object>();
            foreach (ParameterInfo parameter in this.MethodInfo.GetParameters())
            {
                arguments.Add(parameters[parameter.Name]);
            }
            return this.ActionExecutor.Execute(controllerContext.Controller, arguments.ToArray());
        }
    }
}