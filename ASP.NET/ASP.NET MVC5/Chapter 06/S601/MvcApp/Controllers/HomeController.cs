using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            NameValueCollection datasource = new NameValueCollection();

            datasource.Add("foo.Name", "Foo");
            datasource.Add("foo.PhoneNo", "123456789");
            datasource.Add("foo.EmailAddress", "Foo@gmail.com");

            datasource.Add("foo.Address.Province", "江苏");
            datasource.Add("foo.Address.City", "苏州");
            datasource.Add("foo.Address.District", "工业园区");
            datasource.Add("foo.Address.Street", "星湖街328号");

            NameValueCollectionValueProvider valueProvider =  new NameValueCollectionValueProvider(datasource, CultureInfo.InvariantCulture);
            return View(valueProvider);
        }
    }
}