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

            dataSource.Add("foo.Name", "张三");
            dataSource.Add("foo.PhoneNo", "123456789");
            dataSource.Add("foo.EmailAddress", "zhangsan@gmail.com");
            dataSource.Add("foo.Address.Province", "江苏");
            dataSource.Add("foo.Address.City", "苏州");
            dataSource.Add("foo.Address.District", "工业园区");
            dataSource.Add("foo.Address.Street", "星湖街328号");

            dataSource.Add("bar.Name", "李四");
            dataSource.Add("bar.PhoneNo", "987654321");
            dataSource.Add("bar.EmailAddress", "lisi@gmail.com");
            dataSource.Add("bar.Address.Province", "江苏");
            dataSource.Add("bar.Address.City", "苏州");
            dataSource.Add("bar.Address.District", "工业园区");
            dataSource.Add("bar.Address.Street", "金鸡湖路328号");


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