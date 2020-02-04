using Arch.Core;
using Arch.Core.Constants;
using Arch.Core.Enums;
using Arch.Data.GenericRepository;
using Arch.Data.UnitofWork;
using Arch.Dto.ListDto;
using Arch.Dto.ListedDto;
using Arch.Dto.SingleDto;
using Arch.Service.Interfaces;
using Arch.Utilities.Manager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
namespace Arch.Service.Services
{
    public class WorkService : IWorkService
    {
        internal readonly IGenericRawSql<object> _objectRawSql;
        private readonly IGenericRepository<Task> _taskRepository;
        private readonly IGenericRepository<TaskMedia> _taskMediaRepository;
        private readonly IGenericRepository<TaskHistory> _taskHistoryRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<Person> _personRepository;
        private readonly IGenericRepository<Project> _projectRepository;
        private readonly IGenericRepository<LookupList> _lookupListRepository;
        private readonly IGenericRepository<Media> _mediaRepository;
        private readonly IGenericRepository<Unit> _unitRepository;
        public WorkService(IUnitofWork uow)
        {
            _taskRepository = uow.GetRepository<Task>();
            _taskMediaRepository = uow.GetRepository<TaskMedia>();
            _taskHistoryRepository = uow.GetRepository<TaskHistory>();
            _commentRepository = uow.GetRepository<Comment>();
            _personRepository = uow.GetRepository<Person>();
            _projectRepository = uow.GetRepository<Project>();
            _lookupListRepository = uow.GetRepository<LookupList>();
            _mediaRepository = uow.GetRepository<Media>();
            _unitRepository = uow.GetRepository<Unit>();
            _objectRawSql = uow.GetRawSql<object>();
        }

        public List<TaskListDto> GetPageableTasks(int pageSize, int pageNumber, int? assigned, int? requestedBy, int? unitId, int? projectId, int? taskId, int? taskStatusId, string title)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(TaskListDto),
                @"select * from work.GetTaskList({2},{3},{4},{5},{6},{7},{8})  a 
                 order by a.ARequestedDate desc, a.Assigned asc, a.Queue asc OFFSET {0} * ({1} - 1) ROWS FETCH NEXT {0} ROWS ONLY ",
                pageSize, pageNumber, assigned, requestedBy, unitId, projectId, taskId, taskStatusId, title).Cast<TaskListDto>().ToList();

            var resultCount = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(int),
                "select count(a.Id) from work.GetTaskList({0},{1},{2},{3},{4},{5},{6})  a ", assigned, requestedBy, unitId, projectId, taskId, taskStatusId, title).Cast<int>().SingleOrDefault();

            if (result.Count > 0) result[0].ResultCount = resultCount;
            return result;
        }
        public List<TaskListDto> GetTasksByAssignedMe(int assigned)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(TaskListDto),
            @"select * from work.GetTasks({0}) a 
            where IIF(a.TaskStatusId = 38,IIF(year(a.ADueDate) = year(getdate()) and month(a.ADueDate) = month(getdate()),1,0),1)=1 
            order by IIF(a.ADueDate is not null ,a.ADueDate ,a.ARequestedDate) desc, a.Assigned asc, a.Queue asc",
            assigned).Cast<TaskListDto>().ToList();
            return result;
        }
        public List<TaskListDto> GetTaskByTaskId(int taskId)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(TaskListDto),
            "select * from work.GetTaskList(null,null,null,null,{0},-1,null)",
            taskId).Cast<TaskListDto>().ToList();
            return result;
        }
        public List<TaskListDto> GetFilterableTasks(int? assigned, int? requestedBy, int? unitId, int? projectId, int? taskId, int? taskStatusId, string title)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(TaskListDto),
            "select * from work.GetTaskList({0},{1},{2},{3},{4},{5},{6}) a where IIF(a.TaskStatusId = 38,IIF(year(a.ADueDate) = year(getdate()) and month(a.ADueDate) = month(getdate()),1,0),1)=1  order by IIF(a.ADueDate is not null ,a.ADueDate ,a.ARequestedDate) desc, a.Assigned asc, a.Queue asc",
            assigned, requestedBy, unitId, projectId, taskId, taskStatusId, title).Cast<TaskListDto>().ToList();
            return result;
        }
        public object GetTaskComments(int taskId)
        {
            var result = (from a in _taskRepository.GetAll()
                          join b in _commentRepository.GetAll() on a.Id equals b.TaskId
                          join c in _personRepository.GetAll() on b.CommentedBy equals c.Id
                          where a.Id == taskId
                          select new
                          {
                              FullName = c.Name + " " + c.Surname,
                              Comment = b.Description,
                              CreatedDate = b.CommentedDate,
                          }).ToList().Select(p => new
                          {
                              FullName = UtilityManager.ToTitleCase(p.FullName),
                              p.Comment,
                              NameDate = UtilityManager.GetNameableDate(p.CreatedDate),
                          }).ToList();
            return result;
        }
        public Task FindTask(int id)
        {
            return _taskRepository.Find(id);
        }
        public TaskSingleDto GetTaskById(int id)
        {
            var task = _taskRepository.Find(id);
            if (task == null)
                return null;
            var MediaIds = _taskMediaRepository.GetAll().Where(p => p.TaskId == id).Select(p => p.MediaId).ToList();
            var mediaResult = _mediaRepository.GetAll().Where(p => MediaIds.Contains(p.Id)).Select(p => new { p.FileName, p.ContentType }).ToList();
            var result = new TaskSingleDto();
            result.Task = task;
            result.MediaIds = MediaIds;
            result.FileNames = mediaResult.Select(p => p.FileName).ToList();
            result.ContentTypes = mediaResult.Select(p => p.ContentType).ToList();
            return result;
        }
        public object GetProjects()
        {
            return _projectRepository.GetAll().Select(p => new { id = p.Id, text = p.Name }).OrderBy(p => p.text).ToList();
        }
        public List<ComboBoxIdTextDto> GetUnits()
        {
            return _unitRepository.GetAll().OrderBy(p => p.Name).ToList().Select(p => new ComboBoxIdTextDto { text = p.Name, id = p.Id.ToString() }).ToList();
        }
        public List<ComboBoxIdTextDto> GetUnits(int unitTypeId)
        {
            return _unitRepository.GetAll().Where(p => p.UnitTypeId == unitTypeId).OrderBy(p => p.Name).ToList().Select(p => new ComboBoxIdTextDto { text = p.Name, id = p.Id.ToString() }).ToList();
        }
        public List<ComboBoxIdTextDto> GetDaireMudurlukUnits()
        {
            return _unitRepository.GetAll().Where(p => p.UnitTypeId == UnitTypes.Mudurluk || p.UnitTypeId == UnitTypes.DaireBaskanligi).OrderBy(p => p.Name).ToList().Select(p => new ComboBoxIdTextDto { text = p.Name, id = p.Id.ToString() }).ToList();
        }
        public void InsertTask(Task entity)
        {
            _taskRepository.Insert(entity);
        }
        public void InsertProject(Project entity)
        {
            _projectRepository.Insert(entity);
        }
        public void UpdateProject(Project entity)
        {
            _projectRepository.Update(entity);
        }
        public Project FindProject(int id)
        {
            return _projectRepository.Find(id);
        }
        public object GetUnitProjects(int? unitId)
        {
            var result = (from a in _projectRepository.GetAll()
                          join b in _unitRepository.GetAll() on a.UnitId equals b.Id
                          where unitId != null ? a.UnitId == unitId : 1 == 1
                          orderby a.Name ascending
                          select new
                          {
                              ProjectName = a.Name,
                              UnitName = b.Name,
                              ProjectId = a.Id,
                              UnitId = b.Id
                          }).ToList();
            return result;
        }
        public List<ComboBoxIdTextDto> GetUnitProjectList(int unitId)
        {
            var result = (from a in _projectRepository.GetAll()
                          where a.UnitId == unitId
                          orderby a.Name ascending
                          select a).ToList().Select(p => new ComboBoxIdTextDto
                          {
                              id = p.Id.ToString(),
                              text = p.Name
                          }).ToList();
            return result;
        }
        public List<ComboBoxIdTextDto> GetUnitPersons(int unitId)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(ComboBoxIdTextDto),
            @"select id= Convert(varchar,a.Id),text= (a.Name+' '+a.Surname) +' ['+
            (select Convert(varchar,coalesce(sum(IIF(t.TaskStatusId = 38 or t.TaskStatusId = 39,1,0)),0))+'/'+
            Convert(varchar,coalesce(count(Id),0))
            from work.Task t where t.Assigned = a.Id)+']'
            from common.Person a 
            inner join common.GetUnitsByUnitId({0}) b on b.Id = a.UnitId
            order by text asc", unitId).Cast<ComboBoxIdTextDto>().ToList();
            return result;
        }
        public void UpdateTask(Task entity)
        {
            _taskRepository.Update(entity);
        }
        public void InsertTaskHistory(TaskHistory entity)
        {
            _taskHistoryRepository.Insert(entity);
        }
        public void InsertTaskComment(Comment entity)
        {
            _commentRepository.Insert(entity);
        }
        public object GetTaskHistory(int taskId)
        {
            var result = (from a in _taskRepository.GetAll()
                          join b in _taskHistoryRepository.GetAll() on a.Id equals b.TaskId
                          join c in _lookupListRepository.GetAll() on b.TaskStatusId equals c.Id
                          join d in _personRepository.GetAll() on b.Assigned equals d.Id into ds
                          from d in ds.DefaultIfEmpty()
                          join e in _personRepository.GetAll() on b.CreatedBy equals e.Id
                          where a.Id == taskId
                          orderby b.CreatedDate descending
                          select new
                          {
                              Assigned = d == null ? "Atanacak Biri" : d.Name + " " + d.Surname,
                              CreatedBy = e.Name + " " + e.Surname,
                              CreatedDate = b.CreatedDate,
                              TaskStatusName = c.Name
                          }).ToList().Select(p => new
                          {
                              p.Assigned,
                              p.CreatedBy,
                              CreatedDate = UtilityManager.GetNameableDate(p.CreatedDate),
                              p.TaskStatusName
                          }).ToList();
            return result;
        }
        public List<WorkFlowListDto> GetWorkFlow(int? assigned, int? requestedBy, int? unitId)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(WorkFlowListDto),
            "select top 50 * from work.GetWorkFlow({0},{1},{2}) a order by a.ADate desc", assigned, requestedBy, unitId).Cast<WorkFlowListDto>().ToList().Select(
            p => new WorkFlowListDto
            {
                ADateString = UtilityManager.GetNameableDate(p.ADate),
                Assigned = p.Assigned,
                CreatedBy = p.CreatedBy,
                MessageType = p.MessageType,
                TaskId = p.TaskId,
                TaskStatusId = p.TaskStatusId,
                Text = p.Text,
                Title = p.Title
            }).ToList();
            return result;
        }
        public string GetTaskFolowerNames(int taskId)
        {
            var result = _taskRepository.Find(taskId);
            var followerSplitList = result.Followers != null ? result.Followers.Split(',').ToList().Select(int.Parse).ToList() : null;
            if (followerSplitList == null)
                return "";
            var followerNameList = _personRepository.GetAll().Where(p => followerSplitList.Contains(p.Id)).Select(p => p.Name + " " + p.Surname).ToList();
            return string.Join(", ", followerNameList);
        }
        public List<ComboBoxLabelValueDto> GetTaskStatusCounts(int currentUnitId)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(ComboBoxLabelValueDto),
            @"select Value = COUNT(b.Name) ,Label = b.Name 
            from work.Task a  
            inner join common.LookupList b on b.Id = a.TaskStatusId
            inner join common.Person c on c.Id = a.Assigned
            inner join common.GetUnitsByUnitId({0}) d on d.Id = c.UnitId
            group by b.Name 
            order by COUNT(b.Name) desc", currentUnitId).Cast<ComboBoxLabelValueDto>().ToList();
            return result;
        }
        public List<PersonTasktStatusCountsListDto> GetPersonTasktStatusCounts(DateTime? startDate, DateTime? endDate, int currentUnitId)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(PersonTasktStatusCountsListDto),
            "select * from work.GetPersonTasktStatusCounts({0},{1},{2}) a where a.Toplam is not null order by a.Tamamlanan desc",
            startDate, endDate, currentUnitId).Cast<PersonTasktStatusCountsListDto>().ToList();
            return result;
        }
        public List<PersonTaskListDto> GetPersonTaskList(int? projectId, int? unitId, int? personId, int? taskStatusId, DateTime? startDate, DateTime? endDate)
        {
            var result = _objectRawSql.Execute(StaticParams.DefaultConnstr, typeof(PersonTaskListDto),
            "select * from work.GetPersonTaskList({0},{1},{2},{3},{4},{5}) a " +
            "order by a.RequestedDate desc",
            projectId, unitId, personId, taskStatusId, startDate, endDate).Cast<PersonTaskListDto>().ToList();
            return result;
        }
    }
}