using MvcApp.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            NameValueCollection dataSource = new NameValueCollection();

            dataSource.Add("contacts.index", "foo");
            dataSource.Add("contacts.index", "bar");

            dataSource.Add("contacts[foo].Key", "张三");
            dataSource.Add("contacts[foo].Value.Name", "张三");
            dataSource.Add("contacts[foo].Value.PhoneNo", "123456789");
            dataSource.Add("contacts[foo].Value.EmailAddress", "zhangsan@gmail.com");
            dataSource.Add("contacts[foo].Value.Address.Province", "江苏");
            dataSource.Add("contacts[foo].Value.Address.City", "苏州");
            dataSource.Add("contacts[foo].Value.Address.District", "工业园区");
            dataSource.Add("contacts[foo].Value.Address.Street", "星湖街328号");

            dataSource.Add("contacts[bar].Key", "李四");
            dataSource.Add("contacts[bar].Value.Name", "李四");
            dataSource.Add("contacts[bar].Value.PhoneNo", "987654321");
            dataSource.Add("contacts[bar].Value.EmailAddress", "lisi@gmail.com");
            dataSource.Add("contacts[bar].Value.Address.Province", "江苏");
            dataSource.Add("contacts[bar].Value.Address.City", "苏州");
            dataSource.Add("contacts[bar].Value.Address.District", "工业园区");
            dataSource.Add("contacts[bar].Value.Address.Street", "金鸡湖路328号");

            this.ValueProvider = new NameValueCollectionValueProvider(dataSource, CultureInfo.CurrentCulture);
        }

        public object Index()
        {
            return this.InvokeAction("DemoAction");
        }

        public ActionResult DemoAction(IDictionary<string, Contact> contacts)
        {
            var contactArray = contacts.ToArray();
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            foreach (var item in contacts)
            {
                string address = string.Format("{0}省{1}市{2}{3}", item.Value.Address.Province, item.Value.Address.City, item.Value.Address.District, item.Value.Address.Street);
                arguments.Add(string.Format("contacts[\"{0}\"].Name", item.Key), item.Value.Name);
                arguments.Add(string.Format("contacts[\"{0}\"].PhoneNo", item.Key), item.Value.PhoneNo);
                arguments.Add(string.Format("contacts[\"{0}\"].EmailAddress", item.Key), item.Value.EmailAddress);
                arguments.Add(string.Format("contacts[\"{0}\"].Address", item.Key), address);
            }

            return View("Arguments", arguments);
        }
    }
}