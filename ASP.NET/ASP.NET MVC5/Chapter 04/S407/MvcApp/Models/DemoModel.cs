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

        [AllowHtml]
        public string Bar { get; set; }
    }
}