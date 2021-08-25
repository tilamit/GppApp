using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using GppApp.Interface;
using OnlineRevision.Repository;
using GppApp.DbContext;

namespace GppApp.Controllers
{
    public class AuthController : Controller
    {
        private AuthorizeInterface _authRepository;

        public AuthController()
        {
            _authRepository = new AuthorizeRepository(new DbContext.GppAppEntities());
        }

        public AuthController(AuthorizeRepository authsRepository)
        {
            _authRepository = authsRepository;
        }

        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            if(_authRepository.Login(email, pass) != "UserName or Password invalid!")
            {
                string[] val = _authRepository.Login(email, pass).Split('~');
                Session["userName"] = val[0];
                Session["userId"] = val[1];

                //Bangladesh Time
                DateTime utcTime = DateTime.UtcNow;
                TimeZoneInfo BdZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
                DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, BdZone);

                //Track user login
                TrackUsers aTrackUsers = new TrackUsers();
                aTrackUsers.UserId = Convert.ToInt32(val[1]);
                aTrackUsers.LoginTime = localDateTime;

                _authRepository.TrackAllUsers(aTrackUsers);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ViewBag.ErrorMessage = "UserName or Password invalid!";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("userId");
            Session.Remove("userName");
            Session.Abandon();

            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        public ActionResult Logout(string logout)
        {
            Session.Remove("userId");
            Session.Remove("userName");
            Session.Abandon();

            return RedirectToAction("Login", "Auth");
        }

        public ActionResult Registration()
        {
            return View();
        }
    }
}