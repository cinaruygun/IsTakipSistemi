using Arch.Service.Interfaces;
using Arch.Data.UnitofWork;
using Arch.Web.Framework.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mime;
using Arch.Core;
using System;
using Newtonsoft.Json;
using System.Text;
namespace Arch.Web.Controllers
{
    public class BaseController : Controller
    {
        public readonly ILogService _logService;
        public readonly IUnitofWork _uow;
        public BaseController(
            IUnitofWork uow,
            ILogService logService)
        {
            _uow = uow;
            _logService = logService;
        }
        public ActionResult AjaxMessage(string title, string content, MessageTypes state)
        {
            Response.StatusDescription = state.ToString();
            return Content(content + "##" + title, MediaTypeNames.Text.Plain);
        }
        public ActionResult AjaxSuccess()
        {
            return AjaxMessage(MessageTitleTypes.Tebrikler, "İşlem başarıyla gerçeleşti.", MessageTypes.success);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var validationErrors = (from item in ModelState.Values
                                    from error in item.Errors
                                    select error.ErrorMessage).ToList();
            if (validationErrors.Count > 0 && Request.IsAjaxRequest() && validationErrors[0] != "")
            {
                filterContext.Result = AjaxMessage(MessageTitleTypes.Uyari, validationErrors[0], MessageTypes.warning);
            }
        }
    }
}