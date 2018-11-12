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
            ModelMetadata employeeMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => new Employee(), typeof(Employee));
            ModelMetadata salaryMetadata = employeeMetadata.Properties.FirstOrDefault(p => p.PropertyName == "Salary");
            IEnumerable<ModelValidator> validators = salaryMetadata.GetValidators(ControllerContext);
            return View(validators.ToArray());
        }
    }
}