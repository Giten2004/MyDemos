using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class RuleBasedController : Controller
    {
        private static Dictionary<Type, ControllerDescriptor> controllerDescriptors = new Dictionary<Type, ControllerDescriptor>();
        public ControllerDescriptor ControllerDescriptor
        {
            get
            {
                ControllerDescriptor controllerDescriptor;
                if (controllerDescriptors.TryGetValue(this.GetType(), out controllerDescriptor))
                {
                    return controllerDescriptor;
                }
                lock (controllerDescriptors)
                {
                    if (!controllerDescriptors.TryGetValue(this.GetType(), out controllerDescriptor))
                    {
                        controllerDescriptor = new ReflectedControllerDescriptor(this.GetType());
                        controllerDescriptors.Add(this.GetType(), controllerDescriptor);
                    }
                    return controllerDescriptor;
                }
            }
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            SetValidationRule();
            return base.BeginExecuteCore(callback, state);
        }
        protected override void ExecuteCore()
        {
            SetValidationRule();
            base.ExecuteCore();
        }
        private void SetValidationRule()
        {
            string actionName = this.ControllerContext.RouteData.GetRequiredString("action");
            ActionDescriptor actionDescriptor = this.ControllerDescriptor.FindAction(this.ControllerContext, actionName);
            if (null != actionDescriptor)
            {
                ValidationRuleAttribute validationRuleAttribute = actionDescriptor.GetCustomAttributes(true).OfType<ValidationRuleAttribute>().FirstOrDefault() ??
                    this.ControllerDescriptor.GetCustomAttributes(true).OfType<ValidationRuleAttribute>().FirstOrDefault() ??
                    new ValidationRuleAttribute(string.Empty);
                this.ControllerContext.RouteData.DataTokens.Add("ValidationRuleName",validationRuleAttribute.RuleName);
            }
        }
    }
}