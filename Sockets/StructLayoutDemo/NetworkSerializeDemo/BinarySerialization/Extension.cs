using System;
using System.Linq.Expressions;

namespace NetworkSerializeDemo
{
    /// <summary>
    /// extensions for expression tree
    /// </summary>
    internal static partial class Extension
    {
        #region read

        /// <summary>
        /// get property value
        /// </summary>
        /// <typeparam name="TObjectType">type of the object</typeparam>
        /// <typeparam name="TPropertyType">type of the property</typeparam>
        /// <param name="obj">object</param>
        /// <param name="property">property</param>
        /// <returns></returns>
        public static TPropertyType ExtGetPropertyValueByName<TObjectType, TPropertyType>(this TObjectType obj, dynamic property)
        {
            var propertyinfo = ConvertDynamicToPropertyInfo(obj, property);
           
            if (propertyinfo == null)
            {
                return default(TPropertyType);
            }
            var objectinstance = Expression.Parameter(typeof (TObjectType));
            var instanceCast = Expression.Convert(objectinstance, propertyinfo.ReflectedType);
            var propertyAccess = Expression.Property(instanceCast, propertyinfo);
            var castPropertyValue = Expression.Convert(propertyAccess, typeof (TPropertyType));
            var lambda = Expression.Lambda<Func<TObjectType, TPropertyType>>(castPropertyValue, objectinstance);
            return lambda.Compile()(obj);
        }

        #endregion

        #region write

        /// <summary>
        /// set the property (by name)
        /// </summary>
        /// <typeparam name="TObjectType">type of object</typeparam>
        /// <param name="obj">object</param>
        /// <param name="propertyvalue">value</param>
        /// <param name="property">property</param>
        public static void ExtSetPropertyValue<TObjectType>(this TObjectType obj, dynamic propertyvalue, dynamic property)
        {
            var propertyinfo = ConvertDynamicToPropertyInfo(obj, property);

            if (propertyinfo == null)
            {
                return;
            }
            
            var method = propertyinfo.GetSetMethod();
            
            //var fastinvoker = GetMethodInvoker(method);
            //fastinvoker(obj, new[] { propertyvalue });
            CreateExecutor(method)(obj, new[] { propertyvalue });
        }

        #endregion

        /// <summary>
        /// create the instance
        /// </summary>
        /// <typeparam name="T">instance type</typeparam>
        /// <param name="objtype">instance type</param>
        /// <returns></returns>
        public static T ExtCreateInstance<T>(this Type objtype)
        {
            var newexp = Expression.New(objtype);
            var lambda = Expression.Lambda<Func<T>>(newexp, null);
            return lambda.Compile()();
        }

        /// <summary>
        /// | operator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static T ExtOr<T>(this T x, T y)
        {
            var xvalue = Expression.Parameter(typeof (T));
            var yvalue = Expression.Parameter(typeof (T));
            var oroperator = Expression.Or(xvalue, yvalue);
            var lambda = Expression.Lambda<Func<T, T, T>>(oroperator, xvalue, yvalue);
            return lambda.Compile()(x, y);
        }
    }
}