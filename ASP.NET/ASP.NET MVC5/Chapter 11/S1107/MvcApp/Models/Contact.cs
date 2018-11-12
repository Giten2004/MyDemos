using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Contact
    {
        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("电话号码")]
        public string PhoneNo { get; set; }

        [DisplayName("电子邮箱地址")]
        public string EmailAddress { get; set; }
    }
}