using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class HandleExceptionAttribute: FilterAttribute, IExceptionFilter
    {
        public string ExceptionPolicy { get; private set; }
        public HandleErrorActionInvoker HandleErrorActionInvoker { get; private set; }
        public HandleExceptionAttribute(string excptionPolicy)
        {
            this.ExceptionPolicy = excptionPolicy;
            this.HandleErrorActionInvoker = new HandleErrorActionInvoker();
        }

        public void OnException(ExceptionContext filterContext)
        {
            //利用EntLib的EHAB进行异常处理，并获取错误消息和最后抛出的异常
            filterContext.ExceptionHandled = true;
            Exception exceptionToThrow;
            string errorMessage;
            try
            {
                ExceptionHandlingSettings settings = ExceptionHandlingSettings.GetExceptionHandlingSettings(ConfigurationSourceFactory.Create());
                settings.BuildExceptionManager().HandleException(filterContext.Exception, this.ExceptionPolicy, out exceptionToThrow);
                errorMessage = System.Web.HttpContext.Current.GetErrorMessage();
            }
            finally
            {
                System.Web.HttpContext.Current.ClearErrorMessage();
            }
            exceptionToThrow = exceptionToThrow ?? filterContext.Exception;
            errorMessage = string.IsNullOrEmpty(errorMessage) ? exceptionToThrow.Message : errorMessage;

            //对于Ajax请求，直接返回一个用于封装异常的JsonResult
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                ExceptionDetail exceptionDetail = new ExceptionDetail(exceptionToThrow) { Message = errorMessage };
                filterContext.Result = new JsonResult { Data = exceptionDetail, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                return;
            }

            //如果设置了匹配的HandleErrorAction，则调用之；
            //否则将Error View呈现出来       
            ActionDescriptor handleErrorActionDescriptor = this.GetHandleErrorAction(filterContext);
            if (null == handleErrorActionDescriptor)
            {
                string controllerName = filterContext.RouteData.GetRequiredString("controller");
                string actionName = filterContext.RouteData.GetRequiredString("action");
                ViewResult viewResult = new ViewResult { ViewName = "Error" };
                viewResult.ViewData.Model = new ExtendedHandleErrorInfo(exceptionToThrow, controllerName, actionName, errorMessage);
                filterContext.Result = viewResult;
            }
            else
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", errorMessage);
                filterContext.Result = this.HandleErrorActionInvoker.InvokeActionMethod(filterContext, handleErrorActionDescriptor);
            }
        }

        protected virtual ActionDescriptor GetHandleErrorAction(ExceptionContext filterContext)
        {
            string actionName = filterContext.RouteData.GetRequiredString("action");
            ControllerDescriptor controllerDescriptor = this.GetControllerDescriptor(filterContext);
            ActionDescriptor actionDescriptor = controllerDescriptor.FindAction(filterContext, actionName);
            HandleErrorActionAttribute attribute = actionDescriptor.GetCustomAttributes(true).OfType<HandleErrorActionAttribute>().FirstOrDefault();
            if (null == attribute)
            {
                return null;
            }
            return controllerDescriptor.FindAction(filterContext, attribute.HandleErrorAction);
        }

        protected virtual ControllerDescriptor GetControllerDescriptor(ControllerContext controllerContext)
        {
            MethodInfo method = typeof(ControllerActionInvoker).GetMethod("GetControllerDescriptor", BindingFlags.Instance | BindingFlags.NonPublic);
            Controller controller = (Controller)controllerContext.Controller;
            return (ControllerDescriptor)method.Invoke(controller.ActionInvoker, new object[]{controllerContext});
        }
    }
}