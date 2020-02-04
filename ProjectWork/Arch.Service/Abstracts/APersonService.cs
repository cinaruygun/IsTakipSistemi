using Arch.Core;
using Arch.Data.GenericRepository;
using Arch.Data.UnitofWork;
namespace Arch.Service.Abstracts
{
    public abstract class APersonService
    {
        internal readonly IUnitofWork _uow;
        internal readonly IGenericRepository<Task> _taskRepository;
        internal readonly IGenericRepository<Lookup> _lookupRepository;
        internal readonly IGenericRepository<Person> _personRepository;
        internal readonly IGenericRawSql<object> _objectRawSql;
        internal APersonService(IUnitofWork uow)
        {
            _uow = uow;
            _taskRepository = uow.GetRepository<Task>();
            _lookupRepository = uow.GetRepository<Lookup>();
            _personRepository = uow.GetRepository<Person>();
            _objectRawSql = uow.GetRawSql<object>();
        }
    }
}