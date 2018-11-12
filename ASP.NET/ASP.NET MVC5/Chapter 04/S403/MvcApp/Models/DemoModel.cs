using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Models
{
    public class DemoModel
    {
        [HiddenInput]
        public string Foo { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Bar { get; set; }

        public string Baz { get; set; }
    }

}