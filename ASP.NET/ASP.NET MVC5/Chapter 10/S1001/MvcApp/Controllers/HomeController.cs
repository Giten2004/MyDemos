using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : AsyncController
    {
        public void ArticleAsync(string name)
        {
            AsyncManager.OutstandingOperations.Increment();
            string path = ControllerContext.HttpContext.Server.MapPath(string.Format(@"\articles\{0}.html", name));
            StreamReader reader = new StreamReader(path);
            reader.ReadToEndAsync().ContinueWith(Task =>
                {
                    AsyncManager.Parameters["content"] = Task.Result;
                    AsyncManager.OutstandingOperations.Decrement();
                    reader.Close();
                });
        }

        public ActionResult ArticleCompleted(string content)
        {
            return Content(content);
        }
    }  
}