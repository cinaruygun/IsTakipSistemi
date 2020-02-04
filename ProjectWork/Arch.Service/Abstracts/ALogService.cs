using Arch.Core;
using Arch.Data.GenericRepository;
using Arch.Data.UnitofWork;
namespace Arch.Service.Abstracts
{
    public abstract class ALogService
    {
        internal readonly IUnitofWork _uow;
        internal readonly IGenericRepository<Person> _personRepository;
        internal readonly IGenericRepository<ExceptionLog> _exceptionLogRepository;
        internal readonly IGenericRepository<RequestLog> _requestLogRepository;
        internal readonly IGenericRepository<TempRequestLog> _tempRequestLogRepository;
        internal ALogService(IUnitofWork uow)
        {
            _uow = uow;
            _personRepository = uow.GetRepository<Person>();
            _exceptionLogRepository = uow.GetRepository<ExceptionLog>();
            _requestLogRepository = uow.GetRepository<RequestLog>();
            _tempRequestLogRepository = uow.GetRepository<TempRequestLog>();
        }
    }
}