using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JaiHindEdutech.Models
{

    public class AuthRequest
    {
        [Required]
        public string AppKey { get; set; }        // AppKey is PlainText, If you pass token, then AppKey is not required
        [Required]
        public string Username { get; set; }
        [Required]
        public string Provider { get; set; }
        public string Password { get; set; }
        public string JwtToken { get; set; }      // Token contains the Appkey in Claim data
        public string JwtAccessCode { get; set; } // This field is mandatory to be passed from Application, whenever JWT token security is enabled.
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public string SourceApp { get; set; } //Used in security token exchange between application; This may not be IDM systems AppKey
        public string TargetApp { get; set; } //Used in security token exchange between application; This may not be IDM systems AppKey
    }
    public class AuthResponse
    {
        public string AppKey { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public bool isAuthenticated { get; set; }
        public bool? IsFirstTimeLogin { get; set; }
        public List<string> Roles { get; set; }
        //public List<ApplicationList> Applications { get; set; }
        public string ErrorMessage { get; set; }
        public int UserId { get; set; }
        public string LogonData { get; set; }
        public string Provider { get; set; }
        public DateTime LastLogOn { get; set; }
        public List<AppRoles> DeptAccess { get; set; } // Access to all application(s) in the department
        public string JWTToken { get; set; } // Set during authentication

        public AuthResponse()
        {

        }
        public AuthResponse(AuthRequest request)
        {
            AppKey = request.AppKey;
            Username = request.Username;
            Provider = request.Provider;
        }
        public AuthResponse(GetUserRolesInputParams request)
        {
            AppKey = request.AppKey;
            Username = request.Username;
        }


    }
    [Serializable]
    public class AppRoles
    {
        public string AppKey { get; set; }
        public List<string> Roles { get; set; }
    }
    public class GetUserRolesInputParams
    {
        [Required]
        public string AppKey { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    public class AuthLogin
    {
        [Required(ErrorMessage = "Please enter username.")]
        [Display(Name = "User name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter Password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}