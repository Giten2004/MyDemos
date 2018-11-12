using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class FooModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (typeof(Foo).IsAssignableFrom(modelType))
            {
                return new FooModelBinder();
            }
            return null;
        }
    }
}