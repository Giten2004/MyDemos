using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace WebApp
{
    public class DefaultModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, string modelName, Type modelType)
        {
            if (modelType.IsValueType || typeof(string) == modelType)
            {
                object instance;
                if (GetValueTypeInstance(controllerContext, modelName, modelType, out instance))
                {
                    return instance;
                };
                return Activator.CreateInstance(modelType);
            }

            object modelInstance = Activator.CreateInstance(modelType);
            foreach (PropertyInfo property in modelType.GetProperties())
            {
                if (!property.CanWrite || (!property.PropertyType.IsValueType && property.PropertyType != typeof(string)))
                {
                    continue;
                }
                object propertyValue;
                if (GetValueTypeInstance(controllerContext, property.Name,
                    property.PropertyType, out propertyValue))
                {
                    property.SetValue(modelInstance, propertyValue, null);
                }
            }
            return modelInstance;
        }

        private bool GetValueTypeInstance(ControllerContext controllerContext, string modelName, Type modelType, out object value)
        {
            Dictionary<string, object> dataSource = new Dictionary<string, object>();

            //数据来源一：HttpContext.Current.Request.Form
            foreach (string key in HttpContext.Current.Request.Form)
            {
                if (dataSource.ContainsKey(key.ToLower()))
                {
                    continue;
                }
                dataSource.Add(key.ToLower(), HttpContext.Current.Request.Form[key]);
            }

            //数据来源二：HttpContext.Current.Request.QueryString
            foreach (string key in HttpContext.Current.Request.QueryString)
            {
                if (dataSource.ContainsKey(key.ToLower()))
                {
                    continue;
                }
                dataSource.Add(key.ToLower(), HttpContext.Current.Request.QueryString[key]);
            }

            //数据来源三：ControllerContext.RequestContext.RouteData.Values
            foreach (var item in controllerContext.RequestContext.RouteData.Values)
            {
                if (dataSource.ContainsKey(item.Key.ToLower()))
                {
                    continue;
                }
                dataSource.Add(item.Key.ToLower(), controllerContext.RequestContext.RouteData.Values[item.Key]);
            }

            //数据来源四：ControllerContext.RequestContext.RouteData.DataTokens
            foreach (var item in controllerContext.RequestContext.RouteData.DataTokens)
            {
                if (dataSource.ContainsKey(item.Key.ToLower()))
                {
                    continue;
                }
                dataSource.Add(item.Key.ToLower(), controllerContext.RequestContext.RouteData.DataTokens[item.Key]);
            }

            if (dataSource.TryGetValue(modelName.ToLower(), out value))
            {
                value = Convert.ChangeType(value, modelType);
                return true;
            }
            return false;
        }
    }
}
