using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AgeRangeAttribute : RangeAttribute, IClientValidatable
    {
        public AgeRangeAttribute(int minimum, int maximum)
            : base(minimum, maximum)
        { }

        public override bool IsValid(object value)
        {
            DateTime? birthDate = value as DateTime?;
            if (null == birthDate)
            {
                return true;
            }
            DateTime age = new DateTime(DateTime.Today.Ticks - birthDate.Value.Ticks);
            return (int)this.Minimum <= age.Year && age.Year <= (int)this.Maximum;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, this.Minimum, this.Maximum);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string errorMessage = FormatErrorMessage("");
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "agerange",
                ErrorMessage = errorMessage
            };
            rule.ValidationParameters.Add("minage", this.Minimum);
            rule.ValidationParameters.Add("maxage", this.Maximum);
            yield return rule;
        }
    }
}