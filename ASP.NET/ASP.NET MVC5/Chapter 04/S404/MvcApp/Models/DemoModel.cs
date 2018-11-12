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
        [DataType(DataType.EmailAddress)]
        public string Foo { get; set; }

        [DataType("Barcode")]
        public string Bar { get; set; }

        public string Baz { get; set; }

        [DisplayFormat(HtmlEncode = false)]
        public string Qux { get; set; }
    }
}