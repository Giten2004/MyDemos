using MvcApp.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class ResourcesController : Controller
    {
        [Route("resources/{name}/{culture:culture=zh-cn}")]
        public string GetString(string name, string culture)
        {
            CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
                return Resources.ResourceManager.GetString(name.ToLower());
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = currentUICulture;
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }
    }

}