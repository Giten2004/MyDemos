using System;
using System.Collections.Generic;
using System.Linq;
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
            NinjectDependencyResolver dependencyResolver= (NinjectDependencyResolver)DependencyResolver.Current;
            //1. 默认创建的ActionInvoker
            yield return this.CreateActionInvoker();

            //2. 为Dependency注册针对IActionInvoker的类型映射
            dependencyResolver.Register<IActionInvoker, SyncActionInvoker>();
            yield return this.CreateActionInvoker();

            //3. 为Dependency注册针对IAsyncActionInvoker的类型映射
            dependencyResolver.Register<IAsyncActionInvoker, AsyncActionInvoker>();
            yield return this.CreateActionInvoker();
        }
    }
}