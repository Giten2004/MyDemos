using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Employee
    {
        [Display(Name = "ID")]
        public string Id { get; private set; }

        [Display(Name = "姓名")]
        public string Name { get; private set; }

        [Display(Name = "性别")]
        public string Gender { get; private set; }

        [Display(Name = "出生日期")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; private set; }

        [Display(Name = "部门")]
        public string Department { get; private set; }

        public Employee(string id, string name, string gender, DateTime birthDate, string department)
        {
            this.Id = id;
            this.Name = name;
            this.Gender = gender;
            this.BirthDate = birthDate;
            this.Department = department;
        }
    }
}