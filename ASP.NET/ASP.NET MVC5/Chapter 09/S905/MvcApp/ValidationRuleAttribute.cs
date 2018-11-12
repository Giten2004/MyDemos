using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidationRuleAttribute : Attribute
    {
        public string RuleName { get; private set; }
        public ValidationRuleAttribute(string ruleName)
        {
            this.RuleName = ruleName;
        }
    }
}