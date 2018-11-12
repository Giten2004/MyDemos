using MvcApp.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApp.Models
{
    public class Person : IDataErrorInfo
    {
        [DisplayName("姓名")]
        public string Name { get; set; }

        [DisplayName("性别")]
        public string Gender { get; set; }

        [DisplayName("年龄")]
        public int? Age { get; set; }

        [ScaffoldColumn(false)]
        public string Error { get; private set; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name":
                        {
                            if (string.IsNullOrEmpty(this.Name))
                            {
                                return "'姓名'是必需字段";
                            }
                            return null;
                        }
                    case "Gender":
                        {
                            if (string.IsNullOrEmpty(this.Gender))
                            {
                                return "'性别'是必需字段";
                            }
                            else if (!new string[] { "M", "F" }.Any(g => string.Compare(this.Gender, g, true) == 0))
                            {
                                return "'性别'必须是'M','F'之一";
                            }
                            return null;
                        }
                    case "Age":
                        {
                            if (null == this.Age)
                            {
                                return "'年龄'是必需字段";
                            }
                            else if (this.Age > 25 || this.Age < 18)
                            {
                                return "'年龄'必须在18到25周岁之间";
                            }
                            return null;
                        }
                    default: return null;

                }
            }
        }
    }
}