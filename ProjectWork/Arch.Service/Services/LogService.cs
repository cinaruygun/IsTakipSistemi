using Arch.Core;
using Arch.Core.Enums;
using Arch.Data.UnitofWork;
using Arch.Service.Abstracts;
using Arch.Service.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Arch.Service.Services
{
    public class LogService : ALogService, ILogService
    {
        public LogService(IUnitofWork uow) : base(uow) { }

        public void InsertRequestLog(RequestLog requestLog)
        {
            _requestLogRepository.Insert(requestLog);
        }
        public void InsertTempRequestLog(TempRequestLog entity)
        {
            _tempRequestLogRepository.Insert(entity);
        }
        public void InsertExceptionLog(ExceptionLog exceptionLog)
        {
            _exceptionLogRepository.Insert(exceptionLog);
        }
        public void UpdateExceptionLog(ExceptionLog exceptionLog)
        {
            _exceptionLogRepository.Update(exceptionLog);
        }
        public ExceptionLog GetExceptionLog(int hresult)
        {
            return _exceptionLogRepository.GetAll().Where(p => p.HResult == hresult).SingleOrDefault();
        }
        public void InsertForbiddenExceptionLog(int personId, string forbiddenType)
        {
            var req = HttpContext.Current.Request;
            var exceptionLog = new ExceptionLog
            {
                BrowserInfo = req.Browser.Browser + "##" + req.Browser.Platform + "##" + (req.UserLanguages.Length != 0 ? req.UserLanguages[0] : ""),
                CreatedBy = personId,
                CreatedDate = DateTime.Now,
                ExceptionUrl = req.RequestContext.RouteData.Values["controller"].ToString() + "/" + req.RequestContext.RouteData.Values["action"].ToString(),
                IpAdress = req.ServerVariables["REMOTE_ADDR"],
                IsForbidden = true,
                Message = forbiddenType,
                RequestId = null
            };
            _exceptionLogRepository.Insert(exceptionLog);
        }
        public object GetExceptionLogs()
        {
            var result = (from a in _personRepository.GetAll()
                          join b in _exceptionLogRepository.GetAll() on a.Id equals b.CreatedBy
                          orderby b.CreatedDate descending
                          select new
                          {
                              Fullname = a.Name + " " + a.Surname,
                              b.IpAdress,
                              b.Message,
                              b.ExceptionUrl,
                              b.HResult,
                              b.CreatedDate,
                              b.ErrorCount,
                          }).ToList().Select(p => new
                          {
                              p.Fullname,
                              p.IpAdress,
                              p.Message,
                              p.ExceptionUrl,
                              p.HResult,
                              p.ErrorCount,
                              CreatedDate = p.CreatedDate.ToString("dd.MM.yyyy HH:mm"),
                          }).ToList();
            return result;
        }
    }
}