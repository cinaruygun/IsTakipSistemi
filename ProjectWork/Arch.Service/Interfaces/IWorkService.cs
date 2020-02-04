using Arch.Core;
using Arch.Dto.ListDto;
using Arch.Dto.ListedDto;
using Arch.Dto.SingleDto;
using System;
using System.Collections.Generic;
namespace Arch.Service.Interfaces
{
    public interface IWorkService
    {
        List<ComboBoxIdTextDto> GetDaireMudurlukUnits();
        List<TaskListDto> GetPageableTasks(int pageSize, int pageNumber, int? assigned, int? requestedBy, int? unitId, int? projectId, int? taskId, int? taskStatusId, string title);
        object GetTaskComments(int taskId);
        object GetProjects();
        List<ComboBoxIdTextDto> GetUnits();
        List<ComboBoxIdTextDto> GetUnits(int unitTypeId);
        Task FindTask(int id);
        TaskSingleDto GetTaskById(int id);
        void InsertTask(Task entity);
        void UpdateTask(Task entity);
        void InsertTaskHistory(TaskHistory entity);
        void InsertTaskComment(Comment entity);
        object GetTaskHistory(int taskId);
        List<TaskListDto> GetFilterableTasks(int? assigned, int? requestedBy, int? unitId, int? projectId, int? taskId, int? taskStatusId, string title);
        List<WorkFlowListDto> GetWorkFlow(int? assigned, int? requestedBy, int? unitId);
        List<TaskListDto> (int assigned);
        List<TaskListDto> GetTaskByTaskId(int taskId);
        string GetTaskFolowerNames(int taskId);
        List<ComboBoxLabelValueDto> GetTaskStatusCounts(int currentUnitId);
        List<PersonTasktStatusCountsListDto> GetPersonTasktStatusCounts(DateTime? startDate, DateTime? endDate, int currentUnitId);
        void InsertProject(Project entity);
        object GetUnitProjects(int? unitId);
        Project FindProject(int id);
        void UpdateProject(Project entity);
        List<ComboBoxIdTextDto> GetUnitProjectList(int unitId);
        List<ComboBoxIdTextDto> GetUnitPersons(int unitId);
        List<PersonTaskListDto> GetPersonTaskList(int? projectId, int? unitId, int? personId, int? taskStatusId, DateTime? startDate, DateTime? endDate);
    }
}