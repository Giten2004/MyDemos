using MvcApp.Properties;
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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        public string Name { get; set; }

        [DisplayName("性别")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources))]
        [Domain("M", "F", "m", "f", ErrorMessageResourceName = "Domain", ErrorMessageResourceType = typeof(Resources))]
        public string Gender { get; set; }

        [DisplayName("年龄")]
        [Range(18, 25, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(Resources))]
        public int? Age { get; set; }
    }
}