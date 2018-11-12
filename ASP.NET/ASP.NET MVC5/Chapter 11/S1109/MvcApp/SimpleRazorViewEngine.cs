using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class SimpleRazorViewEngine : IViewEngine
    {
        private string[] viewLocationFormats = new string[] { 
            "~/Views/{1}/{0}.cshtml", 
            "~/Views/{1}/{0}.vbhtml", 
            "~/Views/Shared/{0}.cshtml", 
            "~/Views/Shared/{0}.vbhtml" };
        private string[] areaViewLocationFormats = new string[] { 
            "~/Areas/{2}/Views/{1}/{0}.cshtml", 
            "~/Areas/{2}/Views/{1}/{0}.vbhtml", 
            "~/Areas/{2}/Views/Shared/{0}.cshtml", 
            "~/Areas/{2}/Views/Shared/{0}.vbhtml" };

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return FindView(controllerContext, partialViewName, null, useCache);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            object areaName;
            List<string> viewLocations = new List<string>();
            Array.ForEach(viewLocationFormats, format =>viewLocations.Add(string.Format(format, viewName, controllerName)));
            if (controllerContext.RouteData.Values.TryGetValue("area", out areaName))
            {
                Array.ForEach(areaViewLocationFormats,format => viewLocations.Add(string.Format(format, viewName,controllerName, areaName)));
            }
            foreach (string viewLocation in viewLocations)
            {
                string filePath = controllerContext.HttpContext.Request.MapPath(viewLocation);
                if (File.Exists(filePath))
                {
                    return new ViewEngineResult(new SimpleRazorView(viewLocation),this);
                }
            }
            return new ViewEngineResult(viewLocations);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            IDisposable disposable = view as IDisposable;
            if (null != disposable)
            {
                disposable.Dispose();
            }
        }
    }
}