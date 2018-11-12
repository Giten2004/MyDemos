using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Article(string name)
        {
            string path = ControllerContext.HttpContext.Server.MapPath(string.Format(@"\articles\{0}.html", name));
            using (StreamReader reader = new StreamReader(path))
            {
                return Content(await reader.ReadToEndAsync());
            }
        }
    }
}