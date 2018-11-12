using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class ParameterValidationActionInvoker : ControllerActionInvoker
    {
        protected override object GetParameterValue(ControllerContext controllerContext,ParameterDescriptor parameterDescriptor)
        {
            try
            {
                controllerContext.RouteData.DataTokens.Add(typeof(ParameterDescriptor).FullName, parameterDescriptor);
                return base.GetParameterValue(controllerContext,parameterDescriptor);
            }
            finally
            {
                controllerContext.RouteData.DataTokens.Remove(typeof(ParameterDescriptor).FullName);
            }
        }
    }
}