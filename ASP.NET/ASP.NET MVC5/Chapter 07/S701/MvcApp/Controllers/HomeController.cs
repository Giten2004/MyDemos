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
            dataSource.Add("Foo", "ABC");
            dataSource.Add("Bar", "123");
            dataSource.Add("Baz", "456.01");
            dataSource.Add("Qux", "789.01");
            this.ValueProvider = new NameValueCollectionValueProvider(dataSource, CultureInfo.CurrentCulture);
        }

        public object Index()
        {
            return this.InvokeAction("DemoAction");
        }

        public ActionResult DemoAction(string foo,int bar,[Bind(Prefix = "qux")]double baz)
        {
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            arguments.Add("foo", foo);
            arguments.Add("bar", bar);
            arguments.Add("baz", baz);
            return View("Arguments", arguments);
        }
    }
}