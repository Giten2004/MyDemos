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

            datasource.Add("first[0].Name", "Foo");
            datasource.Add("first[0].PhoneNo", "123456789");
            datasource.Add("first[0].EmailAddress", "Foo@gmail.com");

            datasource.Add("first[1].Name", "Bar");
            datasource.Add("first[1].PhoneNo", "987654321");
            datasource.Add("first[1].EmailAddress", "Bar@gmail.com");

            NameValueCollectionValueProvider valueProvider =new NameValueCollectionValueProvider(datasource,CultureInfo.InvariantCulture);
            return View(valueProvider);
        }
    }



}