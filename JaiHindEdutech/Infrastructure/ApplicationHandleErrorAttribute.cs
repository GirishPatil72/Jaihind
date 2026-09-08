using JaiHindEdutech.Models;
using System;
using System.Net;
using System.Web.Mvc;

namespace JaiHindEdutech.Infrastructure
{
    public class ApplicationHandleErrorAttribute : FilterAttribute, IExceptionFilter
    {
        private const string GenericErrorMessage = "Something went wrong while processing your request. Please try again later.";

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null || filterContext.ExceptionHandled)
            {
                return;
            }

            string errorReference = Guid.NewGuid().ToString("N");
            Exception exception = filterContext.Exception;

            LogException(filterContext, exception, errorReference);

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

            if (IsAjaxRequest(filterContext))
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        result = false,
                        message = GenericErrorMessage,
                        reference = errorReference
                    }
                };

                return;
            }

            filterContext.Controller.TempData["ErrorReference"] = errorReference;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                TempData = filterContext.Controller.TempData
            };
        }

        private static bool IsAjaxRequest(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.IsAjaxRequest();
        }

        private static void LogException(ExceptionContext filterContext, Exception exception, string errorReference)
        {
            string controllerName = null;
            string actionName = null;

            if (filterContext.RouteData.Values["controller"] != null)
            {
                controllerName = filterContext.RouteData.Values["controller"].ToString();
            }

            if (filterContext.RouteData.Values["action"] != null)
            {
                actionName = filterContext.RouteData.Values["action"].ToString();
            }

            ActivityLogHelper.Log(
                activityType: "Error",
                entityName: "Application",
                description: "Unhandled application error. Reference: " + errorReference,
                afterValues: new
                {
                    Reference = errorReference,
                    ExceptionType = exception.GetType().FullName,
                    exception.Message,
                    Details = exception.ToString()
                },
                controllerName: controllerName,
                actionName: actionName);
        }
    }
}