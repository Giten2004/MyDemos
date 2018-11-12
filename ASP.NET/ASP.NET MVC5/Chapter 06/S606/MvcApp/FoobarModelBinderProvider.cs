using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class FoobarModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (typeof(Foo).IsAssignableFrom(modelType))
            {
                return new FooModelBinder();
            }

            if (typeof(Bar).IsAssignableFrom(modelType))
            {
                return new BarModelBinder();
            }
            return null;
        }
    }
}