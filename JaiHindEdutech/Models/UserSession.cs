using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class UserSession
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string LoginRole { get; set; }
        public List<string> Roles { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string UserImage { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        //public List<ApplicationList> ApplictaionList { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// Gets the user session.
        /// </summary>       
        /// <returns>The user session.</returns>
        //public static AppUser GetUserSession()
        //{
        //    AppUser userSession = null;
        //    if (System.Web.HttpContext.Current == null)
        //    {
        //        userSession = new AppUser();
        //        userSession.IDMUserID = 1;
        //        return userSession;
        //    }
        //    userSession = System.Web.HttpContext.Current.Session["AppUser"] as AppUser;
        //    if (userSession == null)
        //    {
        //        userSession = new AppUser();
        //        HttpContext.Current.Session["LoginUser"] = userSession;
        //    }
        //    return userSession;
        //}

        /// <summary>
        /// Sets the user session.
        /// </summary>
        /// <param name="userSession">The user session Object.</param>
        public static void SetUserInSession(UserSession userSession)
        {
            if (userSession != null)
            {
                HttpContext.Current.Session["AppUser"] = userSession;
                HttpContext.Current.Session["AuthonticateUser"] = userSession.UserID;
                HttpContext.Current.Session["UserName"] = userSession.FullName;
                HttpContext.Current.Session["FullName"] = userSession.FullName;

            }
        }

        //public static string GetValueFromSession(string key)
        //{
        //    string value = null;

        //    switch (key.ToLower())
        //    {
        //        case "userid":
        //            value = GetUserSession().IDMUserID.ToString();
        //            break;
        //        case "role":
        //            value = string.Join(",", GetUserSession().Roles);
        //            break;
        //        default:
        //            value = Convert.ToString(HttpContext.Current.Session[key]);
        //            //value = (string)HttpContext.Current.Session[key];
        //            break;
        //    }

        //    return value;
        //}
    }
}