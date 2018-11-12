using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class LoginInfo
    {
        [DisplayName("用户名")]
        [Required(ErrorMessage = "请输入{0}")]
        public string UserName { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "请输入{0}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}