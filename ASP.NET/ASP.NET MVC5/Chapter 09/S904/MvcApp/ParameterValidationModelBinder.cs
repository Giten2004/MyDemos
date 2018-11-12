using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class ParameterValidationModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object model = bindingContext.ModelMetadata.Model = base.BindModel(controllerContext, bindingContext);
            ModelMetadata metadata = bindingContext.ModelMetadata;
            if (metadata.IsComplexType || null == model)
            {
                return model;
            }

            //针对简单类型的Model验证
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (ModelValidationResult result in ModelValidator.GetModelValidator(metadata, controllerContext).Validate(null))
            {
                string key = bindingContext.ModelName;
                if (!dictionary.ContainsKey(key))
                {
                    dictionary[key] = bindingContext.ModelState.IsValidField(key);
                }
                if (dictionary[key])
                {
                    bindingContext.ModelState.AddModelError(key, result.Message);
                }
            }
            return model;
        }
    }
}