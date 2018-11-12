using MvcApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Person : IValidatableObject
    {
        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("性别")]
        public string Gender { get; set; }

        [DisplayName("年龄")]
        public int? Age { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Person person = validationContext.ObjectInstance as Person;
            if (null == person)
            {
                yield break;
            }
            if (string.IsNullOrEmpty(person.Name))
            {
                yield return new ValidationResult("'姓名'是必需字段", new string[] { "Name" });
            }

            if (string.IsNullOrEmpty(person.Gender))
            {
                yield return new ValidationResult("'性别'是必需字段", new string[] { "Gender" });
            }
            else if (!new string[] { "M", "F" }.Any(g => string.Compare(person.Gender, g, true) == 0))
            {
                yield return new ValidationResult("'性别'必须是'M','F'之一", new string[] { "Gender" });
            }

            if (null == person.Age)
            {
                yield return new ValidationResult("'年龄'是必需字段", new string[] { "Age" });
            }
            else if (person.Age > 25 || person.Age < 18)
            {
                yield return new ValidationResult("有效'年龄'必须在18到25周岁之间", new string[] { "Age" });
            }
        }
    }
}