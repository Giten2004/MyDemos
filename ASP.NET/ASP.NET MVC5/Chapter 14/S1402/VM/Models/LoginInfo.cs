using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace VM.Models
{
public class LoginInfo
{
    [Required(ErrorMessage="请输入{0}")]
    [Display(Name = "用户名")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "请输入{0}")]
    [Display(Name = "密码")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
}
