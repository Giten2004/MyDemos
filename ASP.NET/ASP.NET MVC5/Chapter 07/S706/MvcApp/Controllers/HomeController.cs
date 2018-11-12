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

            dataSource.Add("contacts.index", "foo");
            dataSource.Add("contacts.index", "bar");

            dataSource.Add("contacts[foo].Name", "张三");
            dataSource.Add("contacts[foo].PhoneNo", "123456789");
            dataSource.Add("contacts[foo].EmailAddress", "zhangsan@gmail.com");
            dataSource.Add("contacts[foo].Address.Province", "江苏");
            dataSource.Add("contacts[foo].Address.City", "苏州");
            dataSource.Add("contacts[foo].Address.District", "工业园区");
            dataSource.Add("contacts[foo].Address.Street", "星湖街328号");

            dataSource.Add("contacts[bar].Name", "李四");
            dataSource.Add("contacts[bar].PhoneNo", "987654321");
            dataSource.Add("contacts[bar].EmailAddress", "lisi@gmail.com");
            dataSource.Add("contacts[bar].Address.Province", "江苏");
            dataSource.Add("contacts[bar].Address.City", "苏州");
            dataSource.Add("contacts[bar].Address.District", "工业园区");
            dataSource.Add("contacts[bar].Address.Street", "金鸡湖路328号");


            this.ValueProvider = new NameValueCollectionValueProvider(dataSource, CultureInfo.CurrentCulture);
        }

        public object Index()
        {
            return this.InvokeAction("DemoAction");
        }

        public ActionResult DemoAction(IEnumerable<Contact> contacts)
        {
            List<Contact> list = new List<Contact>(contacts);
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            for (int i = 0; i < list.Count; i++)
            {
                string name = list[i].Name;
                string phoneNo = list[i].PhoneNo;
                string emailAddress = list[i].EmailAddress;
                string address = string.Format("{0}省{1}市{2}{3}",
                    list[i].Address.Province, list[i].Address.City,
                    list[i].Address.District, list[i].Address.Street);

                arguments.Add(string.Format("[{0}].Name", i), name);
                arguments.Add(string.Format("[{0}].PhoneNo", i), phoneNo);
                arguments.Add(string.Format("[{0}].EmailAddress", i), emailAddress);
                arguments.Add(string.Format("[{0}].Address", i), address);
            }
            return View("Arguments", arguments);
        }

    }
}