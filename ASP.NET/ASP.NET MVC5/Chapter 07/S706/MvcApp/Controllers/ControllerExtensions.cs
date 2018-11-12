using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public static class ControllerExtensions
    {
        public static object InvokeAction(this Controller controller, string actionName)
        {
            IModelBinder modelBinder = new MyDefaultModelBinder();
            ControllerDescriptor controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(controller.ControllerContext, actionName);
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            foreach (ParameterDescriptor parameterDescriptor in actionDescriptor.GetParameters())
            {
                string modelName = parameterDescriptor.BindingInfo.Prefix ?? parameterDescriptor.ParameterName;
                ModelBindingContext bindingContext = new ModelBindingContext
                {
                    FallbackToEmptyPrefix = parameterDescriptor.BindingInfo.Prefix == null,
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, parameterDescriptor.ParameterType),
                    ModelName = modelName,
                    ModelState = controller.ModelState,
                    ValueProvider = controller.ValueProvider
                };
                object argument = modelBinder.BindModel(controller.ControllerContext, bindingContext);
                arguments.Add(parameterDescriptor.ParameterName, argument);
            }
            return actionDescriptor.Execute(controller.ControllerContext, arguments);
        }
    }
}