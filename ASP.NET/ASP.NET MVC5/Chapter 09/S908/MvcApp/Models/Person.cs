using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Person
    {
        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("出生日期")]
        [AgeRange(18, 25, ErrorMessage = "年龄必须在{0}到{1}周岁之间！")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BirthDate { get; set; }
    }
}