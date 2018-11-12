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

            dataSource.Add("Name", "张三");
            dataSource.Add("PhoneNo", "123456789");
            dataSource.Add("EmailAddress", "zhangsan@gmail.com");

            dataSource.Add("Address.Province", "江苏");
            dataSource.Add("Address.City", "苏州");
            dataSource.Add("Address.District", "工业园区");
            dataSource.Add("Address.Street", "星湖街328号");

            this.ValueProvider = new NameValueCollectionValueProvider(dataSource, CultureInfo.CurrentCulture);
        }

        public object Index()
        {
            return this.InvokeAction("DemoAction");
        }

        public ActionResult DemoAction(Contact foo, Contact bar)
        {
            Dictionary<string, object> arguments = new Dictionary<string, object>();

            arguments.Add("foo.Name", foo.Name);
            arguments.Add("foo.PhoneNo", foo.PhoneNo);
            arguments.Add("foo.EmailAddress", foo.EmailAddress);
            Address address = foo.Address;
            arguments.Add("foo.Address", string.Format("{0}省{1}市{2}{3}", address.Province, address.City, address.District, address.Street));

            arguments.Add("bar.Name", bar.Name);
            arguments.Add("bar.PhoneNo", bar.PhoneNo);
            arguments.Add("bar.EmailAddress", bar.EmailAddress);
            address = bar.Address;
            arguments.Add("bar.Address", string.Format("{0}省{1}市{2}{3}", address.Province, address.City, address.District, address.Street));
            return View("Arguments", arguments);
        }
    }
}