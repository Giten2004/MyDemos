using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class RuleBasedValidatorProvider : DataAnnotationsModelValidatorProvider
    {
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context,IEnumerable<Attribute> attributes)
        {
            object validationRuleName = string.Empty;
            context.RouteData.DataTokens.TryGetValue("ValidationRuleName",out validationRuleName);
            string ruleName = validationRuleName.ToString();
            attributes = this.FilterAttributes(attributes, ruleName);
            return base.GetValidators(metadata, context, attributes);
        }

        private IEnumerable<Attribute> FilterAttributes(IEnumerable<Attribute> attributes, string validationRule)
        {
            var validatorAttributes = attributes.OfType<ValidatorAttribute>();
            var nonValidatorAttributes = attributes.Except(validatorAttributes);
            List<ValidatorAttribute> validValidatorAttributes = new List<ValidatorAttribute>();

            if (string.IsNullOrEmpty(validationRule))
            {
                validValidatorAttributes.AddRange(validatorAttributes.Where(v => string.IsNullOrEmpty(v.RuleName)));
            }
            else
            {
                var groups = from validator in validatorAttributes
                             group validator by validator.GetType();
                foreach (var group in groups)
                {
                    ValidatorAttribute validatorAttribute = group.Where(
                        v => string.Compare(v.RuleName, validationRule, true) ==
                        0).FirstOrDefault();
                    if (null != validatorAttribute)
                    {
                        validValidatorAttributes.Add(validatorAttribute);
                    }
                    else
                    {
                        validatorAttribute = group.Where(v => string.IsNullOrEmpty(v.RuleName)).FirstOrDefault();
                        if (null != validatorAttribute)
                        {
                            validValidatorAttributes.Add(validatorAttribute);
                        }
                    }
                }
            }
            return nonValidatorAttributes.Union(validValidatorAttributes);
        }
    }
}