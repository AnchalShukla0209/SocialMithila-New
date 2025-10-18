using Microsoft.AspNet.SignalR.Hosting;
using SocialMithila.Business.Interface;
using SocialMithila.DataAccess.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMithila.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public IBllLogin _IBllLogin { get; set; }
        public AuthController(IBllLogin objIBllLogin)
        {
            _IBllLogin = objIBllLogin;
        }

        public ActionResult Login(string Username, string Password)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginDTO login)
        {
            var res = _IBllLogin.Login(login);
            Session["UserId"] = res.id;
            Session["UserName"] = res.UserName;
            Session["UserProfile"] = res.UserProfile;
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Registration(RegistrationDTO Registration)
        {
            var res = _IBllLogin.Registration(Registration);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

    }
}