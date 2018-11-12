using MvcApp.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            NameValueCollection dataSource = new NameValueCollection();

            dataSource.Add("foo", "abc");
            dataSource.Add("foo", "ijk");
            dataSource.Add("foo", "xyz");

            dataSource.Add("bar", "123");
            dataSource.Add("bar", "456");
            dataSource.Add("bar", "789");



            this.ValueProvider = new NameValueCollectionValueProvider(dataSource, CultureInfo.CurrentCulture);
        }

        public object Index()
        {
            return this.InvokeAction("DemoAction");
        }

        public ActionResult DemoAction(string[] foo, IEnumerable<int> bar)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            for (int i = 0; i < foo.Length; i++)
            {
                parameters.Add(string.Format("foo[{0}]", i), foo[i]);
            }

            int index = 0;
            foreach (int item in bar)
            {
                parameters.Add(string.Format("bar[{0}]", index++), item);
            }

            return View("Arguments", parameters);
        }
    }
}