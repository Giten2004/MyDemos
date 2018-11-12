using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcApp.Models
{
    public class DemoModel
    {
        [Foobar]
        public object Foobar { get; set; }

        [Range(1, 10)]
        public object Range { get; set; }

        [RegularExpression("...")]
        public object RegularExpression { get; set; }

        [Required]
        public object Required { get; set; }

        [StringLength(10)]
        public object StringLength { get; set; }

        [MembershipPassword]
        public object MembershipPassword { get; set; }

        [Compare("Foobar")]
        public object CompareAttribute { get; set; }

        [FileExtensions(Extensions = ".xml,.doc")]
        public object FileExtensions { get; set; }

        [CreditCard]
        public object CreditCard { get; set; }

        [EmailAddress]
        public object EmailAddress { get; set; }

        [Phone]
        public object Phone { get; set; }

        [Url]
        public object Url { get; set; }

        public ValidatableObject ValidatableObject { get; set; }

        public int ValueType { get; set; }
    }

    public class ValidatableObject : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class FoobarAttribute : ValidationAttribute
    { }
}