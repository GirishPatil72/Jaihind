using JaiHindEdutech.Entity;
using System;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Web;
//using System.Web.JavaScript.Serialization;
using System.Web.Script.Serialization;

namespace JaiHindEdutech.Models
{
    public static class ActivityLogHelper
    {
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer
        {
            MaxJsonLength = int.MaxValue
        };

        public static void Log(
            string activityType,
            string entityName,
            string entityId = null,
            string description = null,
            object beforeValues = null,
            object afterValues = null,
            string controllerName = null,
            string actionName = null,
            string userName = null,
            string roleName = null)
        {
            try
            {
                var context = HttpContext.Current;
                var principal = context != null ? context.User : null;
                var identity = principal != null ? principal.Identity : null;

                var resolvedUserName = !string.IsNullOrWhiteSpace(userName)
                    ? userName
                    : (identity != null && identity.IsAuthenticated ? identity.Name : "Anonymous");

                var resolvedRoleName = !string.IsNullOrWhiteSpace(roleName)
                    ? roleName
                    : GetRoleName(principal);

                var beforeJson = Serialize(beforeValues);
                var afterJson = Serialize(afterValues);

                string url = null;
                string httpMethod = null;
                string ipAddress = null;

                if (context != null && context.Request != null)
                {
                    url = context.Request.Url != null ? context.Request.Url.ToString() : null;
                    httpMethod = context.Request.HttpMethod;
                    ipAddress = context.Request.UserHostAddress;
                }

                using (var db = new JaiHindEduEntitiesNew())
                {
                    db.Database.ExecuteSqlCommand(
                        @"INSERT INTO dbo.UserActivityLogs
                          (UserName, RoleName, ActivityType, EntityName, EntityId, Description, BeforeValues, AfterValues, ControllerName, ActionName, Url, HttpMethod, IpAddress, CreatedOn)
                          VALUES
                          (@UserName, @RoleName, @ActivityType, @EntityName, @EntityId, @Description, @BeforeValues, @AfterValues, @ControllerName, @ActionName, @Url, @HttpMethod, @IpAddress, @CreatedOn)",
                        new SqlParameter("@UserName", (object)resolvedUserName ?? DBNull.Value),
                        new SqlParameter("@RoleName", (object)resolvedRoleName ?? DBNull.Value),
                        new SqlParameter("@ActivityType", (object)activityType ?? DBNull.Value),
                        new SqlParameter("@EntityName", (object)entityName ?? DBNull.Value),
                        new SqlParameter("@EntityId", (object)entityId ?? DBNull.Value),
                        new SqlParameter("@Description", (object)description ?? DBNull.Value),
                        new SqlParameter("@BeforeValues", (object)beforeJson ?? DBNull.Value),
                        new SqlParameter("@AfterValues", (object)afterJson ?? DBNull.Value),
                        new SqlParameter("@ControllerName", (object)controllerName ?? DBNull.Value),
                        new SqlParameter("@ActionName", (object)actionName ?? DBNull.Value),
                        new SqlParameter("@Url", (object)url ?? DBNull.Value),
                        new SqlParameter("@HttpMethod", (object)httpMethod ?? DBNull.Value),
                        new SqlParameter("@IpAddress", (object)ipAddress ?? DBNull.Value),
                        new SqlParameter("@CreatedOn", DateTime.Now));
                }
            }
            catch
            {
                // Never break the business flow because of logging.
            }
        }

        private static string Serialize(object value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return Serializer.Serialize(value);
            }
            catch
            {
                return value.ToString();
            }
        }

        private static string GetRoleName(IPrincipal principal)
        {
            if (principal == null)
            {
                return "Unknown";
            }

            if (principal.IsInRole("Admin"))
            {
                return "Admin";
            }

            if (principal.IsInRole("Staff"))
            {
                return "Staff";
            }

            return "User";
        }
    }
}