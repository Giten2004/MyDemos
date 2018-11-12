using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(this.GetActionInvokers().ToArray());
        }

        public IEnumerable<IActionInvoker> GetActionInvokers()
        {
            NinjectDependencyResolver dependencyResolver = (NinjectDependencyResolver)DependencyResolver.Current;
            yield return this.CreateActionInvoker();

            this.ClearCachedActionInvokers();
            dependencyResolver.Register<IActionInvoker, SyncActionInvoker>();
            yield return this.CreateActionInvoker();

            this.ClearCachedActionInvokers();
            dependencyResolver.Register<IAsyncActionInvoker, AsyncActionInvoker>();
            yield return this.CreateActionInvoker();
        }

        private void ClearCachedActionInvokers()
        {
            PropertyInfo property = typeof(DependencyResolver).GetProperty("CurrentCache", BindingFlags.NonPublic | BindingFlags.Static);
            var cachedActionInvoker = property.GetValue(null, null);
            FieldInfo field = cachedActionInvoker.GetType().GetField("_cache", BindingFlags.NonPublic | BindingFlags.Instance);
            ConcurrentDictionary<Type, object> dictionary = (ConcurrentDictionary<Type, object>)field.GetValue(cachedActionInvoker);
            dictionary.Clear();
        }
    }
}