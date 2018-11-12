using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public abstract class LocalizableViewPage<TModel> : WebViewPage<TModel>
    {
        [Inject]
        public ResourceReader ResourceReader { get; set; }
    }
}