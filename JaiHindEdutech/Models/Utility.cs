using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace JaiHindEdutech.Models
{
    public class Utility
    {
        public static string GetPasswordHash(string plainText)
        {
            PasswordHasher passwordHash = new PasswordHasher();
            return passwordHash.HashPassword(plainText);
        }
        public static bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            PasswordHasher passwordHash = new PasswordHasher();
            bool status;
            switch (passwordHash.VerifyHashedPassword(hashedPassword, inputPassword).ToString())
            {
                case "Success": status = true; break;
                case "Failed": status = false; break;
                default: status = false; break;
            }
            return status;
        }

        //public static bool CheckLoggedInUserRoleByRoleName(string roleName = "", string[] roleList = null)
        //{
        //    List<string> roles = UserSession.GetUserSession().Roles;
        //    if (roles != null && roles.Count > 0)
        //    {
        //        bool IsCompare = false;
        //        string role = roles.First();
        //        if (role == roleName)
        //        {
        //            IsCompare = true;
        //        }
        //        else
        //        {
        //            IsCompare = false;
        //        }

        //        if (roleList != null)
        //        {
        //            if (roleList.Length == 1)
        //            {
        //                IsCompare = roles.Any(R => R.Contains(roleList[0]));
        //            }
        //            else
        //            {
        //                IsCompare = roleList.Contains(role);
        //            }
        //        }

        //        return IsCompare;
        //    }
        //    else
        //        return false;
        //}
    }
}