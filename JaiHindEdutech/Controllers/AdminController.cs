using JaiHindEdutech.Entity;
using JaiHindEdutech.Infrastructure;
using JaiHindEdutech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace JaiHindEdutech.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private const string UsersModuleKey = "Users";
        private const string StudentsModuleKey = "Students";
        private const string FeeReceiptsModuleKey = "FeeReceipts";
        private const string CoachingCentresModuleKey = "CoachingCentres";
        private string CurrentUserName
        {
            get { return User != null && User.Identity != null && User.Identity.IsAuthenticated ? User.Identity.Name : string.Empty; }
        }
        [PermissionAuthorize(Right = "View")]
        public ActionResult Index()
        {
            var model = new UserModel().GetUserList();
            return View(model);
        }

        [HttpGet]
        [PermissionAuthorize(Right = "Create")]
        public ActionResult CreateUser()
        {
            var model = new CreateUserViewModel
            {
                IsActive = true
            };

            BindRoles();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(CreateUserViewModel model)
        {
            if (new UserModel().IsUserNameExists(model.UserName))
            {
                ModelState.AddModelError(nameof(model.UserName), "Username already exists.");
            }

            if (!ModelState.IsValid)
            {
                BindRoles(model.SelectedRoleId);
                return View(model);
            }

            var userModel = new UserModel
            {
                UserName = model.UserName,
                PasswordHash = Utility.GetPasswordHash(model.Password),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActive = model.IsActive,
                SelectedRoleId = model.SelectedRoleId
            };

            if (!userModel.CreateNewUser(userModel))
            {
                ModelState.AddModelError("", "User could not be created.");
                BindRoles(model.SelectedRoleId);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [PermissionAuthorize(Right = "Edit")]
        public ActionResult EditUser(int id)
        {
            var user = new UserModel().GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var model = new EditUserViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                SelectedRoleId = user.SelectedRoleId,
                IsActive = user.IsActive ?? true
            };

            BindRoles(model.SelectedRoleId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if (new UserModel().IsUserNameExists(model.UserName, model.UserId))
            {
                ModelState.AddModelError(nameof(model.UserName), "Username already exists.");
            }

            if (!ModelState.IsValid)
            {
                BindRoles(model.SelectedRoleId);
                return View(model);
            }

            var existing = new UserModel().GetUserById(model.UserId);
            if (existing == null)
            {
                return HttpNotFound();
            }

            var userModel = new UserModel
            {
                UserId = model.UserId,
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsActive = model.IsActive,
                SelectedRoleId = model.SelectedRoleId,
                PasswordHash = string.IsNullOrWhiteSpace(model.Password)
                    ? existing.PasswordHash
                    : Utility.GetPasswordHash(model.Password)
            };

            if (!userModel.UpdateUser(userModel))
            {
                ModelState.AddModelError("", "User could not be updated.");
                BindRoles(model.SelectedRoleId);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [PermissionAuthorize(Right = "Create,Edit")]
        public ActionResult UserRights(int id)
        {
            using (var db = new JaiHindEduEntitiesNew())
            {
                var user = db.AspNetUsers.FirstOrDefault(x => x.UserId == id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var existing = db.UserPermissions.Where(x => x.UserId == id && x.IsActive == true).ToList();

                var model = new UserRightsViewModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FullName = string.Join(" ", new[] { user.FirstName, user.LastName }.Where(x => !string.IsNullOrWhiteSpace(x))),
                    Items = BuildModuleList().Select(m =>
                    {
                        var permission = existing.FirstOrDefault(x => x.ModuleKey == m.ModuleKey);
                        return new UserRightItemViewModel
                        {
                            ModuleKey = m.ModuleKey,
                            ModuleName = m.ModuleName,
                            CanView = permission != null && permission.CanView == true,
                            CanCreate = permission != null && permission.CanCreate == true,
                            CanEdit = permission != null && permission.CanEdit == true
                        };
                    }).ToList()
                };

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRights(UserRightsViewModel model)
        {
            using (var db = new JaiHindEduEntitiesNew())
            {
                var user = db.AspNetUsers.FirstOrDefault(x => x.UserId == model.UserId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var existing = db.UserPermissions.Where(x => x.UserId == model.UserId).ToList();
                db.UserPermissions.RemoveRange(existing);

                foreach (var item in model.Items ?? new List<UserRightItemViewModel>())
                {
                    db.UserPermissions.Add(new UserPermission
                    {
                        UserId = model.UserId,
                        ModuleKey = item.ModuleKey,
                        CanView = item.CanView,
                        CanCreate = item.CanCreate,
                        CanEdit = item.CanEdit,
                        IsActive = true,
                        CreatedBy = CurrentUserName,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = CurrentUserName,
                        ModifiedOn = DateTime.Now
                    });
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        [PermissionAuthorize(Right = "Create")]
        public ActionResult CreateCoachingCentre()
        {
            CoachingCentreModel model = new CoachingCentreModel();
            return View(model);
        }
        [PermissionAuthorize(Right = "Edit")]
        public ActionResult EditCoachingCentre()
        {
            return View();
        }

        private void BindRoles(int? selectedRoleId = null)
        {
            ViewBag.Roles = new SelectList(new UserModel().GetRoleList(), "RoleId", "Name", selectedRoleId);
        }

        private List<ModuleInfo> BuildModuleList()
        {
            var modules = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsPublic && !t.IsAbstract && typeof(Controller).IsAssignableFrom(t))
                .SelectMany(controllerType =>
                {
                    var controllerName = controllerType.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                        ? controllerType.Name.Substring(0, controllerType.Name.Length - "Controller".Length)
                        : controllerType.Name;

                    return controllerType
                        .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                        .Where(m => !m.IsSpecialName)
                        .Where(m => !m.GetCustomAttributes(typeof(NonActionAttribute), true).Any())
                        .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType))
                        .Where(m => !m.GetCustomAttributes(typeof(ChildActionOnlyAttribute), true).Any())
                        .Where(m => m.GetCustomAttributes(typeof(PermissionAuthorizeAttribute), true).Any())
                        .Select(actionMethod => new ModuleInfo
                        {
                            ControllerName = controllerName,
                            ActionName = actionMethod.Name,
                            ModuleKey = controllerName + "/" + actionMethod.Name,
                            ModuleName = controllerName + " / " + actionMethod.Name
                        });
                })
                .Distinct(new ModuleInfoComparer())
                .OrderBy(x => x.ControllerName)
                .ThenBy(x => x.ActionName)
                .ToList();

            return modules;
        }

        private sealed class ModuleInfo
        {
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public string ModuleKey { get; set; }
            public string ModuleName { get; set; }
        }

        private sealed class ModuleInfoComparer : IEqualityComparer<ModuleInfo>
        {
            public bool Equals(ModuleInfo x, ModuleInfo y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return string.Equals(x.ModuleKey, y.ModuleKey, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(ModuleInfo obj)
            {
                return (obj == null || obj.ModuleKey == null ? string.Empty : obj.ModuleKey)
                    .ToLowerInvariant()
                    .GetHashCode();
            }
        }

        [HttpGet]
        [PermissionAuthorize(Right = "View")]
        public ActionResult ActivityLogs()
        {
            using (var db = new JaiHindEduEntitiesNew())
            {
                var model = db.UserActivityLogs
                    .OrderByDescending(x => x.CreatedOn)
                    .ThenByDescending(x => x.UserActivityLogId)
                    .Take(500)
                    .ToList();

                return View(model);
            }
        }
    }
}