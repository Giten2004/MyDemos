using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class ParameterValidationModelValidatorProvider :DataAnnotationsModelValidatorProvider
    {
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context,
            IEnumerable<Attribute> attributes)
        {
            object descriptor;
            if (metadata.ContainerType == null && context.RouteData.DataTokens.TryGetValue(typeof(ParameterDescriptor).FullName, out descriptor))
            {
                ParameterDescriptor parameterDescriptor = (ParameterDescriptor)descriptor;
                DisplayAttribute displayAttribute = parameterDescriptor.GetCustomAttributes(true).OfType<DisplayAttribute>().FirstOrDefault()
                    ?? new DisplayAttribute { Name = parameterDescriptor.ParameterName };
                metadata.DisplayName = displayAttribute.Name;
                var addedAttributes = parameterDescriptor.GetCustomAttributes(true).OfType<Attribute>();
                return base.GetValidators(metadata, context,attributes.Union(addedAttributes));
            }
            else
            {
                return base.GetValidators(metadata, context, attributes);
            }
        }
    }
}