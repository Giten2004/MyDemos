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
        [ReadOnly(true)]
        public string Foo { get; set; }

        [Editable(true)]
        [ReadOnly(true)]
        public string Bar { get; set; }

        [Editable(false)]
        [ReadOnly(false)]
        public string Baz { get; set; }
    }

}