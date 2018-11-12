using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcApp
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
    public abstract class ValidatorAttribute : ValidationAttribute
    {
        private object typeId;
        public string RuleName { get; set; }
        public override object TypeId
        {
            get { return typeId ?? (typeId = new object()); }
        }
    }
}