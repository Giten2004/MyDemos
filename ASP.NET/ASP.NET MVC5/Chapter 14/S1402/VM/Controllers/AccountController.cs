using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VM.Models;
using System.Web.Security;
using VM.Mvc.Extensions;
using System.Web.Routing;

namespace VM.Controllers
{
    public class AccountController : VmController
    {
        public ActionResult Login()
        {
            return View(new LoginInfo { UserName = "Artech" });
        }

        [HttpPost]
        [HandleErrorAction("OnLoginError")]
        public ActionResult Login(LoginInfo loginInfo, string returnUrl = "")
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (Membership.ValidateUser(loginInfo.UserName, loginInfo.Password))
            {
                FormsAuthentication.SetAuthCookie(loginInfo.UserName, false);
                if (string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect("/");
                }
                return Redirect(returnUrl);
            }
            else
            {
                //这段代码为了演示“自动化”异常处理
                if (Membership.FindUsersByName(loginInfo.UserName).Count == 0)
                {
                    throw new InvalidUserNameException("Specified user account does not exists!");
                }
                throw new InvalidPasswordException("Specified password is incorrect!");
            }
        }

        /// <summary>
        /// 用于处理从Action方法Login中抛出的异常
        /// </summary>
        /// <remarks>
        /// 1、Login页面会被呈现
        /// 2、输入的用户名会显示在用户名文本框中
        /// 3、错误信息会显示在ValidationSummary中
        /// </remarks>
        public ActionResult OnLoginError(LoginInfo loginInfo)
        {
            return View(loginInfo);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            string loginUrl = RouteTable.Routes.GetVirtualPath(ControllerContext.RequestContext, "Login", null).VirtualPath;
            return Redirect(loginUrl);
        }
    }
}