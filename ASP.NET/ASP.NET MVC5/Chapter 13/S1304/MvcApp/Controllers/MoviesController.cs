using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    [RouteArea("vedio")]
    [RoutePrefix("movies")]
    public class MoviesController : Controller
    {
        [HttpGet]
        [Route("{genres:alpha}/{language:culture=en}", Name = "FindMovies")]
        public ActionResult FindMovies(string genres, string language)
        {
            throw new NotImplementedException();
        }
    }
}