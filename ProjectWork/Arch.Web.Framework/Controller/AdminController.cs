using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Arch.Service.Interfaces;
using Arch.Data.UnitofWork;
using Arch.Web.Framework.Enums;
using Arch.Core;
using Arch.Dto.SingleDto;
using System.Web.Security;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;

namespace Arch.Web.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(
            IUnitofWork uow,
            ILogService logService)
            : base(uow, logService)
        {
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var forbiddenType = Accesses.IsLogin();
            if (forbiddenType != ForbiddenAccessTypes.UnForbidden && forbiddenType != ForbiddenAccessTypes.IsLogout)
            {
                //requestlog icinde tutulmali
                _logService.InsertForbiddenExceptionLog(Accesses.PersonId, ((int)ForbiddenAccessTypes.Database).ToString());
                _uow.SaveChanges();
            }

            if (!(User.Identity.IsAuthenticated && forbiddenType == ForbiddenAccessTypes.UnForbidden && forbiddenType != ForbiddenAccessTypes.IsLogout))
            {
                var c = requestContext.RouteData.Values["controller"];
                var a = requestContext.RouteData.Values["action"];
                requestContext.RouteData.Values["controller"] = "Login";
                requestContext.RouteData.Values["action"] = "Logout";
                Response.Redirect("/login/logout?r=/" + c + "/" + a);
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (Request.UserHostAddress != "::1")
            {
                var c = filterContext.RouteData.Values["controller"];
                var a = filterContext.RouteData.Values["action"];
                var requestLogs = new TempRequestLog
                {
                    CreatedDate = DateTime.Now,
                    RequestUrl = "/" + c + "/" + a,
                    PersonId = Accesses.PersonId,
                    IpAddress = Request.UserHostAddress,
                };
                _logService.InsertTempRequestLog(requestLogs);
                _uow.SaveChanges();
            }

            if (filterContext.Exception != null)
            {
                var exceptionLog = new ExceptionLog
                 {
                     StackTrace = filterContext.Exception.InnerException == null ? filterContext.Exception.StackTrace : filterContext.Exception.InnerException.StackTrace,
                     Message = filterContext.Exception.InnerException == null ? filterContext.Exception.Message : filterContext.Exception.InnerException.Message,
                     ExceptionUrl = filterContext.HttpContext.Request.RawUrl.ToString(),
                     IpAdress = Request.UserHostAddress ?? "0",
                     HResult = filterContext.Exception.HResult,
                     BrowserInfo = "Name : " + Request.Browser.Browser + ", Type : " + Request.Browser.Type + ", Version : " + Request.Browser.Version,
                     CreatedBy = Accesses.PersonId,
                     CreatedDate = DateTime.Now,
                     ErrorCount = 1,
                 };
                _logService.InsertExceptionLog(exceptionLog);
                _uow.SaveChanges();

                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.Status = "500 Internal Server Error";
                filterContext.Result = AjaxMessage("Hata", "Hata No :" + filterContext.Exception.HResult + "<br/> Hata Mesajı : " + (filterContext.Exception.InnerException == null ? filterContext.Exception.Message : filterContext.Exception.InnerException.Message), MessageTypes.danger);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}