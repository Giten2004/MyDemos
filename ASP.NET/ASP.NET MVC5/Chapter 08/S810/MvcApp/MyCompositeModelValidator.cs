using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class MyCompositeModelValidator : ModelValidator
    {
        public MyCompositeModelValidator(ModelMetadata metadata, ControllerContext controllerContext)
            : base(metadata, controllerContext)
        { }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            bool isPropertiesValid = true;
            foreach (ModelMetadata propertyMetadata in Metadata.Properties)
            {
                foreach (ModelValidator validator in propertyMetadata.GetValidators(this.ControllerContext))
                {
                    IEnumerable<ModelValidationResult> results = validator.Validate(this.Metadata.Model);
                    if (results.Any())
                    {
                        isPropertiesValid = false;
                    }
                    foreach (ModelValidationResult result in results)
                    {
                        string key = (propertyMetadata.PropertyName ?? "") + "." + (result.MemberName ?? "");                        
                        yield return new ModelValidationResult
                        {

                            MemberName = key.Trim('.'),
                            Message = result.Message
                        };
                    }
                }
            }

            if (isPropertiesValid)
            {
                foreach (ModelValidator validator in Metadata.GetValidators(this.ControllerContext))
                {
                    IEnumerable<ModelValidationResult> results = validator.Validate(Metadata.Model);
                    foreach (ModelValidationResult result in results)
                    {
                        yield return result;
                    }
                }
            }
        }
    }
}