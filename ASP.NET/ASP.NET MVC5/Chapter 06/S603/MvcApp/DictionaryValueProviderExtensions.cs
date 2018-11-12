using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public static class DictionaryValueProviderExtensions
    {
        public static Dictionary<string, ValueProviderResult> GetDataSource<TValue>(this DictionaryValueProvider<TValue> valueProvider)
        {
            FieldInfo valuesField = typeof(DictionaryValueProvider<TValue>).GetField("_values", BindingFlags.Instance | BindingFlags.NonPublic);
            return (Dictionary<string, ValueProviderResult>)valuesField.GetValue(valueProvider);
        }
    }
}