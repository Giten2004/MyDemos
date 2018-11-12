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
            ModelMetadataInfo metadataInfo = new ModelMetadataInfo(typeof(DemoModel),
                metadata => metadata.DisplayName,
                metadata => metadata.Description,
                metadata => metadata.ShortDisplayName,
                metadata => metadata.Watermark,
                metadata => metadata.Order);
            return View(metadataInfo);

        }
    }
}