using JaiHindEdutech.Entity;
using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JaiHindEdutech.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserExist"] == null)
                return View();
            else if (Session["UserExist"] != null)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Login(AuthLogin authLogin)
        {
            bool Success = false;
            if (ModelState.IsValid)
            {
                string hashpassword = Utility.GetPasswordHash(authLogin.Password);
                using (JaiHindEduEntitiesNew db = new JaiHindEduEntitiesNew())
                {
                    var user = db.AspNetUsers.Where(x => x.UserName == authLogin.Username && x.IsActive == true).SingleOrDefault();
                    if (user != null)
                    {
                        Success = Utility.VerifyPassword(user.PasswordHash, authLogin.Password);
                        if (Success)
                        {
                            List<string> userRoles = (from ar in db.AspNetUserRoles
                                                      join r in db.AspNetRoles on ar.RoleId equals r.RoleId
                                                      where ar.UserId == user.UserId
                                                      select r.Name).ToList();

                            Session["LoginUser"] = user.UserName;
                            AppUser.SetUserInSession(
                                new AppUser
                                {
                                    Roles = userRoles,
                                    LoginRole = userRoles.FirstOrDefault(),
                                    IDMUserID = user.UserId,
                                    UserName = user.UserName,
                                    Email = user.Email,
                                    FullName = user.FirstName + " " + user.LastName,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName
                                }
                            );
                            FormsAuthentication.SetAuthCookie(user.UserName, false);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.InvalidUser = "Invalid Password.";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.InvalidUser = "Invalid UserName.";
                        return View();
                    }
                }
            }
            else
            {
                ViewBag.InvalidUser = "Invalid UserName and Password.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["UserExist"] = null;
            Session["LoginUser"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}