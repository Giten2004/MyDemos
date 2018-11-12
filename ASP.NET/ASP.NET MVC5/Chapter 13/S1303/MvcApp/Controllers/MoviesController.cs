using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    [RoutePrefix("movies")]
    public class MoviesController : Controller
    {
        [Route("{genres:alpha}/{language:culture=en}", Name = "FindMovies")]
        public ActionResult FindMovies(string genres, string language)
        {
            throw new NotImplementedException();
        }
    }
}