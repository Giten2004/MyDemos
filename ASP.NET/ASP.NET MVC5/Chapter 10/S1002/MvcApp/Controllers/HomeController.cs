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
        public Task<ActionResult> Article(string name)
        {
            string path = ControllerContext.HttpContext.Server.MapPath(string.Format(@"\articles\{0}.html", name));
            StreamReader reader = new StreamReader(path);
            return reader.ReadToEndAsync().ContinueWith<ActionResult>(task =>
            {
                reader.Close();
                return Content(task.Result);
            });
        }
    }
}