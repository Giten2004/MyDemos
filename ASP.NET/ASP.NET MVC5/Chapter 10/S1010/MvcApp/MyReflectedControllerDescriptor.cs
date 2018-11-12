using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class MyReflectedControllerDescriptor : ReflectedControllerDescriptor
    {
        public MyReflectedControllerDescriptor(Type controllerType)
            : base(controllerType)
        { }

        public override ActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
        {
            ActionDescriptor actionDescriptor = base.FindAction(controllerContext, actionName);
            ReflectedActionDescriptor reflectedActionDescriptor = actionDescriptor as ReflectedActionDescriptor;
            if (null != reflectedActionDescriptor)
            {
                return new MyReflectedActionDescriptor(reflectedActionDescriptor);
            }
            return actionDescriptor;
        }
    }
}