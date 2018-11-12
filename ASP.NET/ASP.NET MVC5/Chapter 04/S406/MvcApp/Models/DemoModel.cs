using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Models
{
    public class DemoModel
    {
        public string Foo { get; set; }

        [DisplayName("Bar")]
        public string Bar { get; set; }

        [Display(Name = "BAZ", Description = "Desc",
            ShortName = "B", Prompt = "Watermark...", Order = 999)]
        [DisplayName("baz")]
        public string Baz { get; set; }
    }
}