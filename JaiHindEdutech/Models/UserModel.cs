using JaiHindEdutech.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace JaiHindEdutech.Models
{
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
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string FullName { get; set; }
        public string AccountReason { get; set; }
        public string ContactPhone { get; set; }
        public DateTime LastLogOn { get; set; }
        public string Provider { get; set; }

        public int SelectedRoleId { get; set; }
        public string RoleName { get; set; }

        public bool CreateNewUser(UserModel user)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var aspnetUser = new AspNetUser
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PasswordHash = user.PasswordHash,
                        IsActive = user.IsActive ?? true,
                        IsFirstTimeLogin = true,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    };

                    db.AspNetUsers.Add(aspnetUser);
                    db.SaveChanges();

                    if (user.SelectedRoleId > 0)
                    {
                        db.AspNetUserRoles.Add(new AspNetUserRole
                        {
                            UserId = aspnetUser.UserId,
                            RoleId = user.SelectedRoleId
                        });
                        db.SaveChanges();
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateUser(UserModel user)
        {
            try
            {
                using (var db = new JaiHindEduEntitiesNew())
                {
                    var existingUser = db.AspNetUsers.SingleOrDefault(x => x.UserId == user.UserId);
                    if (existingUser == null)
                    {
                        return false;
                    }

                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.UserName = user.UserName;
                    existingUser.Email = user.Email;
                    existingUser.IsActive = user.IsActive ?? true;
                    existingUser.ModifiedOn = DateTime.Now;

                    if (!string.IsNullOrWhiteSpace(user.PasswordHash))
                    {
                        existingUser.PasswordHash = user.PasswordHash;
                    }

                    var existingRoles = db.AspNetUserRoles.Where(x => x.UserId == user.UserId).ToList();
                    if (existingRoles.Any())
                    {
                        db.AspNetUserRoles.RemoveRange(existingRoles);
                    }

                    if (user.SelectedRoleId > 0)
                    {
                        db.AspNetUserRoles.Add(new AspNetUserRole
                        {
                            UserId = existingUser.UserId,
                            RoleId = user.SelectedRoleId
                        });
                    }

                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<AspNetRole> GetRoleList()
        {
            using (var db = new JaiHindEduEntitiesNew())
            {
                return db.AspNetRoles.OrderBy(r => r.Name).ToList();
            }
        }

        public bool IsUserNameExists(string username, int userId = 0)
        {
            using (var db = new JaiHindEduEntitiesNew())
            {
                var query = db.AspNetUsers.Where(x => x.UserName == username);

                if (userId > 0)
                {
                    query = query.Where(x => x.UserId != userId);
                }

                return query.Any();
            }
        }

        public UserModel GetUserById(int userId)
        {
            using (var db = new JaiHindEduEntitiesNew())
            {
                var user = db.AspNetUsers
                    .Where(x => x.UserId == userId)
                    .Select(x => new UserModel
                    {
                        UserId = x.UserId,
                        UserName = x.UserName,
                        EmployeeID = x.EmployeeID,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        PasswordHash = x.PasswordHash,
                        Email = x.Email,
                        EmployeeTitle = x.EmployeeTitle,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        ModifiedOn = x.ModifiedOn
                    })
                    .FirstOrDefault();

                if (user == null)
                {
                    return null;
                }

                var roleId = db.AspNetUserRoles
                    .Where(x => x.UserId == userId)
                    .Select(x => (int?)x.RoleId)
                    .FirstOrDefault();

                if (roleId.HasValue)
                {
                    user.SelectedRoleId = roleId.Value;
                    user.RoleName = db.AspNetRoles
                        .Where(x => x.RoleId == roleId.Value)
                        .Select(x => x.Name)
                        .FirstOrDefault();
                }

                user.FullName = string.Join(" ", new[] { user.FirstName, user.LastName }
                    .Where(x => !string.IsNullOrWhiteSpace(x)));

                return user;
            }
        }

        public List<UserModel> GetUserList()
        {
            var userList = new List<UserModel>();

            using (var db = new JaiHindEduEntitiesNew())
            {
                foreach (var u in db.AspNetUsers.ToList())
                {
                    var user = new UserModel
                    {
                        UserId = u.UserId,
                        UserName = u.UserName,
                        EmployeeID = u.EmployeeID,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        PasswordHash = u.PasswordHash,
                        Email = u.Email,
                        EmployeeTitle = u.EmployeeTitle,
                        IsActive = u.IsActive,
                        CreatedOn = u.CreatedOn,
                        ModifiedOn = u.ModifiedOn
                    };

                    var roleId = db.AspNetUserRoles
                        .Where(x => x.UserId == u.UserId)
                        .Select(x => (int?)x.RoleId)
                        .FirstOrDefault();

                    if (roleId.HasValue)
                    {
                        user.SelectedRoleId = roleId.Value;
                        user.RoleName = db.AspNetRoles
                            .Where(x => x.RoleId == roleId.Value)
                            .Select(x => x.Name)
                            .FirstOrDefault();
                    }

                    user.FullName = string.Join(" ", new[] { user.FirstName, user.LastName }
                        .Where(x => !string.IsNullOrWhiteSpace(x)));

                    userList.Add(user);
                }
            }

            return userList;
        }
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        public int SelectedRoleId { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter username.")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select a role.")]
        public int SelectedRoleId { get; set; }

        public bool IsActive { get; set; }
    }

    public class UserRightItemViewModel
    {
        public string ModuleKey { get; set; }
        public string ModuleName { get; set; }
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class UserRightsViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public List<UserRightItemViewModel> Items { get; set; }
    }
}