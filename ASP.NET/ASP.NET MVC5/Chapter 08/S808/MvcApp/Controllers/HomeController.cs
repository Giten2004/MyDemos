using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Address address = new Address
            {
                Province = "江苏",
                City = "苏州",
                District = "工业园区",
                Street = "星湖街328号"
            };

            Contact contact = new Contact
            {
                Name = "张三",
                PhoneNo = "123456789",
                EmailAddress = "zhangsan@gmail.com",
                Address = address
            };

            ModelMetadata metadata = ModelMetadataProviders.Current.GetMetadataForType(() => contact, typeof(Contact));
            ModelValidator validator = ModelValidator.GetModelValidator(metadata,ControllerContext);
            return View(validator.Validate(contact));
        }
    }
}