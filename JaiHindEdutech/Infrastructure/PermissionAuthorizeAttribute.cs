using JaiHindEdutech.Entity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JaiHindEdutech.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public string ModuleKey { get; set; }
        public string Right { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
            {
                return false;
            }

            if (httpContext == null ||
                httpContext.User == null ||
                httpContext.User.Identity == null ||
                !httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            var userName = httpContext.User.Identity.Name;
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            string routeController = null;
            string routeAction = null;

            if (httpContext.Request != null &&
                httpContext.Request.RequestContext != null &&
                httpContext.Request.RequestContext.RouteData != null &&
                httpContext.Request.RequestContext.RouteData.Values != null)
            {
                var routeValues = httpContext.Request.RequestContext.RouteData.Values;
                var controllerValue = routeValues["controller"];
                var actionValue = routeValues["action"];

                routeController = controllerValue != null ? controllerValue.ToString() : null;
                routeAction = actionValue != null ? actionValue.ToString() : null;
            }

            var key = string.IsNullOrWhiteSpace(ModuleKey)
                ? routeController + "/" + routeAction
                : ModuleKey;

            using (var db = new JaiHindEduEntitiesNew())
            {
                var user = db.AspNetUsers.FirstOrDefault(x => x.UserName == userName && x.IsActive == true);
                if (user == null)
                {
                    return false;
                }

                var permission = db.UserPermissions.FirstOrDefault(x =>
                    x.UserId == user.UserId &&
                    x.ModuleKey == key &&
                    x.IsActive == true);

                if (permission == null)
                {
                    return false;
                }

                var rights = (Right ?? string.Empty)
                    .Split(new[] { ',', '|', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim().ToLowerInvariant())
                    .ToList();

                if (rights.Count == 0)
                {
                    return false;
                }

                return rights.Any(r =>
                    (r == "view" && permission.CanView == true) ||
                    (r == "create" && permission.CanCreate == true) ||
                    (r == "edit" && permission.CanEdit == true) ||
                    (r == "delete" && permission.CanDelete == true));
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "AccessDenied",
                    message = "You are not authorized to take this action."
                }));
        }
    }
}