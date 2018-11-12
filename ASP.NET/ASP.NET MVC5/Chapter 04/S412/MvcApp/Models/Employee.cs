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

        [RadioButtonList("Gender")]
        [DisplayName("性别")]
        public string Gender { get; set; }

        [DropdownList("Education")]
        [DisplayName("学历")]
        public string Education { get; set; }

        [ListBox("Department")]
        [DisplayName("所在部门")]
        public IEnumerable<string> Departments { get; set; }

        [CheckBoxList("Skill")]
        [DisplayName("擅长技能")]
        public IEnumerable<string> Skills { get; set; }
    }

}