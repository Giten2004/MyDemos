using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
             [HandleException("defaultPolicy")]

    public class HomeController : Controller
    {
        static Dictionary<string, string> userAcccounts;
        static HomeController()
        {
            userAcccounts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            userAcccounts.Add("foo", "password");
            userAcccounts.Add("bar", "password");
            userAcccounts.Add("baz", "password");
        }

        public ActionResult Index()
        {
            return View(new LoginInfo());
        }

        [HttpPost]
        [HandleErrorAction("OnIndexError")]
        public string  Index(LoginInfo loginInfo)
        {
            string pwd;
            if (userAcccounts.TryGetValue(loginInfo.UserName, out pwd))
            {
                if (loginInfo.Password != pwd)
                {
                    throw new InvalidPasswordException();
                }
                return "认证成功";
            }
            throw new InvalidUserNameException();
        }

        [HttpPost]
        public ActionResult OnIndexError(LoginInfo loginInfo)
        {
            return View(loginInfo);
        }
    }
}