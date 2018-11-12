using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
public class ActionExecutor
{
    private static Dictionary<MethodInfo, object> delegates = new Dictionary<MethodInfo, object>();
    private static object syncHelper = new object();
    public MethodInfo MethodInfo { get; private set; }

    public ActionExecutor(MethodInfo methodInfo)
    {
        this.MethodInfo = methodInfo;
    }

    public object Execute(ControllerBase controller, object[] arguments)
    {
        object actionOrFunc;
        if (delegates.TryGetValue(this.MethodInfo, out actionOrFunc))
        {
            return this.ExecuteCore(controller, arguments, actionOrFunc);
        }

        lock (syncHelper)
        {
            if (delegates.TryGetValue(this.MethodInfo, out actionOrFunc))
            {
                return this.ExecuteCore(controller, arguments, actionOrFunc);
            }
            actionOrFunc = CreateExecutor(this.MethodInfo);
            delegates[this.MethodInfo] = actionOrFunc;
            return this.ExecuteCore(controller, arguments, actionOrFunc);
        }
    }

    //执行指定的Action<ControllerBase, object[]>或者Func<ControllerBase, object[],object>对象
    protected virtual object ExecuteCore(ControllerBase controller, object[] arguments, object executor)
    {
        Action<ControllerBase, object[]> action = executor as Action<ControllerBase, object[]>;
        if (null != action)
        {
            action(controller, arguments);
            return null;
        }

        Func<ControllerBase, object[], object> func = executor as Func<ControllerBase, object[], object>;
        if (null != func)
        {
            return  func(controller, arguments);
        }

        return null;
    }

    //生成执行目标Action的表达式，并将其编译成Action<ControllerBase, object[]>
    //或者Func<ControllerBase, object[],object>对象
    protected virtual object CreateExecutor(MethodInfo methodInfo)
    {
        ParameterExpression target = Expression.Parameter(typeof(ControllerBase), "controller");
        ParameterExpression arguments = Expression.Parameter(typeof(object[]), "arguments");

        List<Expression> parameters = new List<Expression>();
        ParameterInfo[] paramInfos = methodInfo.GetParameters();
        for (int i = 0; i < paramInfos.Length; i++)
        {
            ParameterInfo paramInfo = paramInfos[i];
            BinaryExpression getElementByIndex = Expression.ArrayIndex(arguments,Expression.Constant(i));
            UnaryExpression convertToParameterType = Expression.Convert(getElementByIndex, paramInfo.ParameterType);
            parameters.Add(convertToParameterType);
        }

        UnaryExpression instanceCast = Expression.Convert(target,methodInfo.ReflectedType);
        MethodCallExpression methodCall = Expression.Call(instanceCast, methodInfo, parameters);

        if (methodInfo.ReflectedType == typeof(void))
        {
            return Expression.Lambda<Action<ControllerBase, object[]>>(methodCall, target, arguments).Compile();
        }
        else
        {
            UnaryExpression convertToObjectType = Expression.Convert(methodCall,typeof(object));
            return Expression.Lambda<Func<ControllerBase, object[], object>>(convertToObjectType, target, arguments).Compile();
        }
    }
}
}