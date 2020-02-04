using Arch.Data.UnitofWork;
using Arch.Service.Interfaces;
using Arch.Utilities.Manager;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
namespace Arch.Web.Controllers
{
    public class DashboardController : AdminController
    {
        private readonly IPersonService _personService;
        public DashboardController(
            IUnitofWork uow,
            ILogService logService,
            IPersonService personService)
            : base(uow, logService)
        {
            _personService = personService;
        }
        public ActionResult Index() { return View(); }
        public ActionResult ExceptionLogs() { return View(); }
        public ActionResult GetExceptionLogs() { return Json(_logService.GetExceptionLogs(), JsonRequestBehavior.AllowGet); }
    }
}