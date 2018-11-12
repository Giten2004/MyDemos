using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class MyDefaultModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string key = bindingContext.ModelName;
            IValueProvider valueProvider = bindingContext.ValueProvider;
            if (valueProvider.ContainsPrefix(bindingContext.ModelName))
            {
                ValueProviderResult valueProviderResult = valueProvider.GetValue(key);
                return this.BindSimpleModel(controllerContext, bindingContext, valueProviderResult);
            }
            return null;
        }

        private object BindSimpleModel(ControllerContext controllerContext, ModelBindingContext bindingContext, ValueProviderResult valueProviderResult)
        {
            SetModelState(bindingContext, valueProviderResult);
            return valueProviderResult.ConvertTo(bindingContext.ModelType);
        }

        private void SetModelState(ModelBindingContext bindingContext, ValueProviderResult valueProviderResult)
        {
            ModelState modelState;
            if (!bindingContext.ModelState.TryGetValue(bindingContext.ModelName, out modelState))
            {
                bindingContext.ModelState.Add(bindingContext.ModelName, modelState = new ModelState());
            }
            modelState.Value = valueProviderResult;
        }
    }
}