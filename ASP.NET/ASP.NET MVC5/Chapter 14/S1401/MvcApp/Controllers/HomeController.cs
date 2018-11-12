using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        [Dependency]
        public ITimeProvider TimeProvider { get; set; }

        public void Index()
        {
            for (int i = 0; i < 3; i++)
            {
                Response.Write(string.Format("{0}: {1: hh:mm:ss}<br/>", "Utc", this.TimeProvider.GetCurrentTime(DateTimeKind.Utc)));
                Thread.Sleep(1000);

                Response.Write(string.Format("{0}: {1: hh:mm:ss}<br/><br/>", "Local", this.TimeProvider.GetCurrentTime(DateTimeKind.Local)));
                Thread.Sleep(1000);
            }
        }
    }  
}