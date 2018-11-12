using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ModelMetadata metadata = new ModelMetadata(ModelMetadataProviders.Current, null, null, typeof(DemoModel), null);
            ViewBag.TemplateNamesAccessor = new Func<ModelMetadata, string, IEnumerable<string>>(GetCandidateTemplates);
            return View(metadata.Properties);
        }

        static IEnumerable<string> GetCandidateTemplates(ModelMetadata modelMetadata, string template)
        {
            Type templateHelpers = Type.GetType("System.Web.Mvc.Html.TemplateHelpers,System.Web.Mvc, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            MethodInfo getViewNames = templateHelpers.GetMethod("GetViewNames", BindingFlags.NonPublic | BindingFlags.Static);
            string[] templates = new string[] { template, modelMetadata.TemplateHint, modelMetadata.DataTypeName };
            return (IEnumerable<string>)getViewNames.Invoke(null, new object[] { modelMetadata, templates });
        }
    }
}