using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class OperationData
    {
        [DisplayName("操作数1")]
        public double Operand1 { get; set; }
        [DisplayName("操作数2")]
        public double Operand2 { get; set; }
        [DisplayName("操作符")]
        public string Operator { get; set; }
        [DisplayName("运算结果")]
        public double Result { get; set; }
    }
}