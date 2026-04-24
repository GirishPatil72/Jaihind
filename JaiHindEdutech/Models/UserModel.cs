using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace JaiHindEdutech.Models
{
    public class AppUser
    {
        public string AppKey { get; set; }
        public int IDMUserID { get; set; }
        public string EmployeeID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmployeeTitle { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public List<string> Roles { get; set; }
        public string FullName { get; set; }
        public string ProviderName { get; set; }
        public string LoginRole { get; set; }
        

        public static void SetUserInSession(AppUser userSession)
        {
            if (userSession != null)
            {
                HttpContext.Current.Session["AppUser"] = userSession;
                HttpContext.Current.Session["AuthonticateUser"] = userSession.IDMUserID;
                HttpContext.Current.Session["UserName"] = userSession.UserName;
                HttpContext.Current.Session["FullName"] = userSession.FirstName + " " + userSession.LastName;
                HttpContext.Current.Session["Roles"]= userSession.Roles;
                HttpContext.Current.Session["LoginRole"] = userSession.LoginRole;
            }
        }
    }

    public class UserRole
    {
        public List<int> UserIdList { get; set; }
        public List<int> RoleIdList { get; set; }
    }

    public class AppUserRole
    {
        public string AppKey { get; set; }
        public string Roles { get; set; }

    }

    public class ApplicationList
    {
        public string ApplicationName { get; set; }
        public string ApplicationURL { get; set; }
    }

    public class UserLoginCount
    {
        public Nullable<int> IDMUserID { get; set; }
        public int LoginCnt { get; set; }
    }

    public class AppUserLoginCount
    {
        public string AppKey { get; set; }
        public string Users { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
    }

    public class IDMUser
    {
        [Required]
        public string AppKey { get; set; }
        public int IDMUserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string EmployeeID { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string EmployeeTitle { get; set; }
        public string Role { get; set; }
        [Required]
        public string Provider { get; set; }
        public string Status { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string ErrorMessage { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RegistrationActivationCode { get; set; }
        public string TemporaryPassword { get; set; }
        public string AccountReason { get; set; }
        public string ContactPhone { get; set; }
    }


    public class Provider
    {
        public string ProviderName { get; set; }
        public string ProviderURL { get; set; }
        public string ServiceUsername { get; set; }
        public string ServicePassword { get; set; }
        public bool Status { get; set; }

    }
    public class UserModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please enter username.")]
        public string UserName { get; set; }
        public string EmployeeID { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Please enter first name.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter last name.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter email.")]
        public string Email { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsFirstTimeLogin { get; set; }
        public string EmployeeTitle { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public List<string> Roles { get; set; }
        //public List<Application> AppList { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string FullName { get; set; }
        public string AccountReason { get; set; }
        public string ContactPhone { get; set; }
        public DateTime LastLogOn { get; set; }
        public string Provider { get; set; }
        //public List<AppRoles> DeptAccess { get; set; } // Access to all application(s) in the department

        string ActiveDirectory = "Active Directory";
        string SQL = "SQL";

        public bool CreateNewUser(UserModel user)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {

                    AspNetUser aspnetUser = new AspNetUser();
                    aspnetUser.FirstName = user.FirstName;
                    aspnetUser.LastName = user.LastName;
                    aspnetUser.UserName = user.UserName;
                    aspnetUser.Email = user.Email;
                    aspnetUser.PasswordHash = user.PasswordHash;
                    aspnetUser.IsActive = true;
                    aspnetUser.IsFirstTimeLogin = true;
                    aspnetUser.CreatedOn = DateTime.Now;
                    aspnetUser.ModifiedOn = DateTime.Now;
                    db.AspNetUsers.Add(aspnetUser);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                throw;
            }
            catch
            {
                throw;
            }
        }



        public bool IsUserActive(string Username, string AppKey, string UserProvider)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var user = GetActiveAppUser(AppKey, Username, UserProvider);
                    return (user == null) ? false : true;

                }
            }
            catch
            {
                throw;
            }
        }
        public AspNetUser GetActiveAppUser(string AppKey, string Username, string UserProvider = null)
        {
            AspNetUser activeUser = null;
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {

                    if (!string.IsNullOrEmpty(UserProvider))
                        activeUser = db.AspNetUsers.Where(x => x.UserName == Username && x.IsActive == true).SingleOrDefault();
                    else
                        activeUser = db.AspNetUsers.Where(x => x.UserName == Username && x.IsActive == true).SingleOrDefault();
                    return activeUser;
                }

            }
            catch
            {
                throw;
            }
        }

        // This is a IDM interface based createuser function
        public bool IsUserExists(UserModel user)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {

                    var userExists = db.AspNetUsers.Where(x => x.UserName == user.UserName).SingleOrDefault();
                    return (userExists == null) ? false : true;

                }
            }
            catch
            {
                throw;
            }
        }

        public List<UserModel> GetUserList()
        {
            List<UserModel> userList = new List<UserModel>();
            StringBuilder sb = new StringBuilder();
            using (var db = new JaiHindEduEntitiesNew())
            {

                foreach (var u in db.AspNetUsers.ToList())
                {
                    UserModel um = new UserModel();
                    um.UserId = u.UserId;
                    um.UserName = u.UserName;
                    um.EmployeeID = u.EmployeeID;
                    um.FirstName = u.FirstName;
                    um.LastName = u.LastName;
                    um.PasswordHash = u.PasswordHash;
                    um.Email = u.Email;
                    um.EmployeeTitle = u.EmployeeTitle;
                    // sb.Clear();
                    //foreach (var up in u.AspNetUserLogins.ToList())
                    //{
                    //    sb.Append(um.ProviderName);
                    //    sb.Append(",");
                    //    sb.Append(up.LoginProvider);
                    //}
                    //um.ProviderName = sb.Length > 0 ? sb.Remove(0, 1).ToString() : string.Empty;
                    //sb.Clear();
                    ////foreach (var r in u.AspNetRoles.ToList())
                    //{
                    //    sb.Append(um.Roles);
                    //    sb.Append(",");
                    //    sb.Append(r.Name);
                    //}
                    //um.Roles = sb.Length > 0 ? sb.Remove(0, 1).ToString() : string.Empty;
                    um.IsActive = u.IsActive;
                    um.CreatedOn = u.CreatedOn;
                    um.ModifiedOn = u.ModifiedOn;
                    userList.Add(um);
                }

            }
            return userList;
        }
    }
}