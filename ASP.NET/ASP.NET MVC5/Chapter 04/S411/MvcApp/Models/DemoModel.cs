using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class DemoModel
    {
        [UIHint("TemplateHint")]
        [DataType("DataTypeName")]
        public int Foo { get; set; }

        [UIHint("TemplateHint")]
        [DataType("DataTypeName")]
        public int? Bar { get; set; }

        [UIHint("TemplateHint")]
        [DataType("DataTypeName")]
        public Baz Baz { get; set; }

        [UIHint("TemplateHint")]
        [DataType("DataTypeName")]
        public IEnumerable<int> Qux { get; set; }
    }
    public class Baz
    { }

}