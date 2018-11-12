using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
public class Person
{
    [DisplayName("姓名")]
    public string Name { get; set; }

    [DisplayName("性别")]
    public string Gender { get; set; }

    [DisplayName("年龄")]
    public int? Age { get; set; }
}

}