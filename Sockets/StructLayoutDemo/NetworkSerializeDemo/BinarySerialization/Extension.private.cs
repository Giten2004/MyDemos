using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace NetworkSerializeDemo
{
    internal static partial class Extension
    {
        /// <summary>
        /// call the method dynamic
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        private static Func<object, object[], object> CreateExecutor(MethodInfo methodInfo)
        {
            var instanceParameter = Expression.Parameter(typeof(object));
            var parametersParameter = Expression.Parameter(typeof(object[]));
            var parameterExpressions = new List<Expression>();
            var paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                var valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }
            Expression instanceCast = methodInfo.IsStatic
                                          ? null
                                          : Expression.Convert(instanceParameter, methodInfo.ReflectedType);
            var methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);
            if (methodCall.Type == typeof(void))
            {
                var lambda = Expression.Lambda<Action<object, object[]>>(methodCall, instanceParameter, parametersParameter);
                var execute = lambda.Compile();
                return (instance, parameters) =>
                       {
                           execute(instance, parameters);
                           return null;
                       };
            }
            else
            {
                var castMethodCall = Expression.Convert(methodCall, typeof(object));
                var lambda = Expression.Lambda<Func<object, object[], object>>(castMethodCall, instanceParameter, parametersParameter);
                return lambda.Compile();
            }
        }

        /// <summary>
        /// check the obj if null or empty
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool NothingCheck<TObject>(this TObject obj)
        {
            var type = typeof(TObject);
            var instance = Expression.Parameter(type);
            var nullconstant = Expression.Constant(null);
            var emptyconstant = Expression.Constant(string.Empty);

            var lambdafornull = Expression.Lambda<Func<TObject, bool>>(Expression.Equal(instance, nullconstant), new[] { instance });
            var lambdaforempty = Expression.Lambda<Func<TObject, bool>>(Expression.Equal(emptyconstant, emptyconstant), new[] { instance });
            if (obj is string)
            {
                lambdaforempty = Expression.Lambda<Func<TObject, bool>>(Expression.Equal(instance, emptyconstant), new[] { instance });
            }

            var equals = Expression.Lambda<Func<TObject, bool>>(Expression.And(lambdaforempty.Body, lambdafornull.Body), new[] { instance });

            return equals.Compile()(obj);
        }

        /// <summary>
        /// conver to the dynamic object to property info (string or propertyinfo)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private static PropertyInfo ConvertDynamicToPropertyInfo(dynamic obj, dynamic property)
        {
            if (property == null)
            {
                return null;
            }
            if (property is string)
            {
                return obj.GetType().GetProperty(property);
            }
            if (property is PropertyInfo)
            {
                return property;
            }
            return null;
        }
    }
}