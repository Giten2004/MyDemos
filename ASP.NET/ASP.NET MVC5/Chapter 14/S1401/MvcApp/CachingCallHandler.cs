using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace MvcApp
{
    public class CachingCallHandler : ICallHandler
    {
        public TimeSpan ExpirationTime { get; private set; }
        internal static TimeSpan DefaultExpirationTime { get; private set; }
        internal static Func<MethodBase, object[], string> CacheKeyGenerator { get; private set; }

        static CachingCallHandler()
        {
            DefaultExpirationTime = new TimeSpan(0, 5, 0);
            Guid prefix = Guid.NewGuid();
            //缓存项Key的格式：{GUID}：{方法返回声明类型名称}{方法名称}：{输入参数HashCode}
            CacheKeyGenerator = (method, inputs) =>
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}:", prefix);
                sb.AppendFormat("{0}:", method.DeclaringType.FullName);
                sb.AppendFormat("{0}:", method.Name);
                if (null != inputs)
                {
                    Array.ForEach(inputs, input =>
                    {
                        string hashCode = (null == input) ? "" : input.GetHashCode().ToString();
                        sb.AppendFormat("{0}:", hashCode);
                    });
                }
                return sb.ToString().TrimEnd(':');
            };
        }

        public CachingCallHandler(TimeSpan? expirationTime = null)
        {
            this.ExpirationTime = expirationTime.HasValue ? expirationTime.Value : DefaultExpirationTime;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            MethodInfo targetMethod = (MethodInfo)input.MethodBase;
            if (targetMethod.ReturnType == typeof(void))
            {
                return getNext()(input, getNext);
            }
            object[] inputs = new object[input.Inputs.Count];
            input.Inputs.CopyTo(inputs, 0);
            string cacheKey = CacheKeyGenerator(targetMethod, inputs);
            object[] cachedResult = HttpRuntime.Cache.Get(cacheKey) as object[];
            if (null == cachedResult)
            {
                IMethodReturn realReturn = getNext()(input, getNext);
                if (null == realReturn.Exception)
                {
                    HttpRuntime.Cache.Insert(cacheKey,new object[] { realReturn.ReturnValue }, null, DateTime.Now.Add(this.ExpirationTime),Cache.NoSlidingExpiration);
                }
                return realReturn;
            }
            return input.CreateMethodReturn(cachedResult[0], new object[] { input.Arguments });
        }

        public int Order { get; set; }
    }
}