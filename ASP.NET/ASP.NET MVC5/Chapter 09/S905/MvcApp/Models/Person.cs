using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Person
    {
        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("性别")]
        public string Gender { get; set; }

        [DisplayName("年龄")]
        [RangeValidator(10, 20, RuleName = "Rule1", ErrorMessage = "{0}必须在{1}和{2}之间！")]
        [RangeValidator(20, 30, RuleName = "Rule2", ErrorMessage = "{0}必须在{1}和{2}之间！")]
        [RangeValidator(30, 40, RuleName = "Rule3", ErrorMessage = "{0}必须在{1}和{2}之间！")]
        public int Age { get; set; }
    }
}