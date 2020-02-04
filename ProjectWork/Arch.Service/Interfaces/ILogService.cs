using Arch.Core;
namespace Arch.Service.Interfaces
{
    public interface ILogService
    {
        void InsertTempRequestLog(TempRequestLog entity);
        void InsertExceptionLog(ExceptionLog exceptionLog);
        void InsertRequestLog(RequestLog requestLog);
        void UpdateExceptionLog(ExceptionLog exceptionLog);
        ExceptionLog GetExceptionLog(int hresult);
        void InsertForbiddenExceptionLog(int personId, string forbiddenType);
        object GetExceptionLogs();
    }
}
