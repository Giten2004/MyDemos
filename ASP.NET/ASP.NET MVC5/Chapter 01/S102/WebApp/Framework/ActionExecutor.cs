using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace WebApp
{
    internal class ActionExecutor
    {
        private static Dictionary<MethodInfo, Func<object, object[], object>> executors = new Dictionary<MethodInfo, Func<object, object[], object>>();
        private static object syncHelper = new object();
        public MethodInfo MethodInfo { get; private set; }

        public ActionExecutor(MethodInfo methodInfo)
        {
            this.MethodInfo = methodInfo;
        }

        public object Execute(object target, object[] arguments)
        {
            Func<object, object[], object> executor;
            if (!executors.TryGetValue(this.MethodInfo, out executor))
            {
                lock (syncHelper)
                {
                    if (!executors.TryGetValue(this.MethodInfo, out executor))
                    {
                        executor = CreateExecutor(this.MethodInfo);
                        executors[this.MethodInfo] = executor;
                    }
                }
            }
            return executor(target, arguments);
        }

        private static Func<object, object[], object> CreateExecutor(MethodInfo methodInfo)
        {
            ParameterExpression target = Expression.Parameter(typeof(object), "target");
            ParameterExpression arguments = Expression.Parameter(typeof(object[]), "arguments");

            List<Expression> parameters = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo paramInfo = paramInfos[i];
                BinaryExpression getElementByIndex = Expression.ArrayIndex(arguments, Expression.Constant(i));
                UnaryExpression convertToParameterType = Expression.Convert(getElementByIndex, paramInfo.ParameterType);
                parameters.Add(convertToParameterType);
            }

            UnaryExpression instanceCast = Expression.Convert(target, methodInfo.ReflectedType);
            MethodCallExpression methodCall = methodCall = Expression.Call(instanceCast, methodInfo, parameters);
            UnaryExpression convertToObjectType = Expression.Convert(methodCall, typeof(object));
            return Expression.Lambda<Func<object, object[], object>>(convertToObjectType, target, arguments).Compile();
        }
    }
}