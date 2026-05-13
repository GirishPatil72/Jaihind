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
    public class AccountController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthLogin authLogin)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.InvalidUser = "Invalid UserName and Password.";
                return View();
            }

            using (JaiHindEduEntitiesNew db = new JaiHindEduEntitiesNew())
            {
                var user = db.AspNetUsers.SingleOrDefault(x => x.UserName == authLogin.Username && x.IsActive == true);

                if (user == null)
                {
                    ViewBag.InvalidUser = "Invalid UserName.";
                    return View();
                }

                bool success = Utility.VerifyPassword(user.PasswordHash, authLogin.Password);
                if (!success)
                {
                    ViewBag.InvalidUser = "Invalid Password.";
                    return View();
                }

                List<string> userRoles = (from ar in db.AspNetUserRoles
                                          join r in db.AspNetRoles on ar.RoleId equals r.RoleId
                                          where ar.UserId == user.UserId
                                          select r.Name).Distinct().ToList();

                string fullName = string.Join(" ", new[] { user.FirstName, user.LastName }
                    .Where(x => !string.IsNullOrWhiteSpace(x)))
                    .Trim();

                SetAuthCookie(user.UserName, fullName, userRoles, false);

                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private void SetAuthCookie(string userName, string fullName, IEnumerable<string> roles, bool isPersistent)
        {
            string roleData = string.Join(",", roles ?? Enumerable.Empty<string>());
            string userData = string.Format("{0}|{1}", fullName ?? string.Empty, roleData);

            var ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                isPersistent,
                userData
            );

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath
            };

            if (isPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }

            Response.Cookies.Add(cookie);
        }
    }
}