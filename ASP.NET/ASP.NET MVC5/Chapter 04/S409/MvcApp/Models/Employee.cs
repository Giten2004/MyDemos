using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Employee
    {
        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("部门")]
        public string Department { get; set; }

        [DisplayName("是否兼职")]
        public bool IsPartTime { get; set; }
    }
}