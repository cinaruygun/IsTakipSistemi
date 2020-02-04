using Arch.Core;
using Arch.Core.Constants;
using Arch.Data.GenericRepository;
using Arch.Data.UnitofWork;
using Arch.Dto.ListedDto;
using Arch.Dto.SingleDto;
using Arch.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Arch.Utilities.Manager;
using Arch.Dto.ListDto;
using Arch.Core.Enums;
namespace Arch.Service.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly IGenericRepository<Parameters> _parametersRepository;
        private readonly IGenericRepository<LookupList> _lookupListRepository;
        private readonly IGenericRepository<Person> _personRepository;
        private readonly IGenericRawSql<object> _objectRawSql;
        private readonly IGenericRepository<Task> _taskRepository;
        private readonly IGenericRepository<Unit> _unitRepository;
        private readonly IGenericRepository<Project> _projectRepository;
        public UtilityService(IUnitofWork uow)
        {
            _parametersRepository = uow.GetRepository<Parameters>();
            _lookupListRepository = uow.GetRepository<LookupList>();
            _taskRepository = uow.GetRepository<Task>();
            _personRepository = uow.GetRepository<Person>();
            _unitRepository = uow.GetRepository<Unit>();
            _projectRepository = uow.GetRepository<Project>();
            _objectRawSql = uow.GetRawSql<object>();
        }

        public int? GetUnitFirstProjectId(int unitId)
        {
            return _projectRepository.GetAll().Where(p => p.UnitId == unitId).Select(p => p.Id).FirstOrDefault();
        }
        public List<LookupList> GetLookupLists()
        {
            return _lookupListRepository.GetAll().ToList();
        }
        public List<Parameters> GetParameters()
        {
            return _parametersRepository.GetAll().ToList();
        }
        public bool AnyEmail(string email)
        {
            return _personRepository.GetAll().Any(p => p.Email == email);
        }
        public int GetTaskCount(int? assigned)
        {
            return _taskRepository.GetAll().Where(p => assigned != null ? p.Assigned == assigned : 1 == 1).Count();
        }
    }
}