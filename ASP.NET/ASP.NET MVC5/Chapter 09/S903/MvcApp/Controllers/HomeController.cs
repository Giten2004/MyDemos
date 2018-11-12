using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dictionary<string, ModelValidator[]> modelValidators = new Dictionary<string, ModelValidator[]>();
            DataAnnotationsModelValidatorProvider validatorProvider = new DataAnnotationsModelValidatorProvider();
            foreach (ModelMetadata metadata in ModelMetadataProviders.Current.GetMetadataForProperties(new DemoModel(), typeof(DemoModel)))
            {
                modelValidators.Add(metadata.PropertyName, validatorProvider.GetValidators(metadata, this.ControllerContext).ToArray());
            }
            return View(modelValidators);
        }
    }
}