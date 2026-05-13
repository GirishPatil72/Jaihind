using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace JaiHindEdutech
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = "sub";
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
            {
                return;
            }

            FormsAuthenticationTicket authTicket;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }

            if (authTicket == null || authTicket.Expired)
            {
                return;
            }

            string fullName = string.Empty;
            string[] roles = Array.Empty<string>();

            var parts = (authTicket.UserData ?? string.Empty).Split('|');
            if (parts.Length > 0)
            {
                fullName = parts[0];
            }

            if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
            {
                roles = parts[1]
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .ToArray();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, authTicket.Name ?? string.Empty),
                new Claim("FullName", fullName)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, "Forms");
            var principal = new ClaimsPrincipal(identity);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identity.Name));

            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
        }

        protected void Application_BeginRequest()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");
        }
    }
}
