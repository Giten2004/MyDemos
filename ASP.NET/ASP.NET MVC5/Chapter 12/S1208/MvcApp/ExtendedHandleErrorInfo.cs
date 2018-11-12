using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class ExtendedHandleErrorInfo : HandleErrorInfo
    {
        public string ErrorMessage { get; private set; }
        public ExtendedHandleErrorInfo(Exception exception,
            string controllerName, string actionName, string errorMessage)
            : base(exception, controllerName, actionName)
        {
            this.ErrorMessage = errorMessage;
        }
    }

}