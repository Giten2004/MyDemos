using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    [RouteArea("vedio")]
    [RoutePrefix("movies")]
    [Route("{action}/{id}", Name = "FindMovies")]
    public class MoviesController : Controller
    {
        public ActionResult Index(string id)
        {
            throw new NotImplementedException();
        }
    }
}