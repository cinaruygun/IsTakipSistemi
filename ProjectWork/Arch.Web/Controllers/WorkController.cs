using Arch.Core;
using Arch.Core.Enums;
using Arch.Data.UnitofWork;
using Arch.Dto.ParamDto;
using Arch.Dto.SingleDto;
using Arch.Service.Interfaces;
using Arch.Web.Framework.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Arch.Web.Controllers
{
    public class WorkController : AdminController
    {
        private readonly IWorkService _workService;
        private readonly IPersonService _personService;
        private readonly IMediaService _mediaService;
        private readonly IUtilityService _utiltiyService;
        public WorkController(
            IWorkService workService,
            IUnitofWork uow,
            IPersonService personService,
            IUtilityService utiltiyService,
            IMediaService mediaService,
            ILogService logService)
            : base(uow, logService)
        {
            _workService = workService;
            _personService = personService;
            _mediaService = mediaService;
            _utiltiyService = utiltiyService;
        }
        public ActionResult Index() { return View(); }
        public ActionResult Board() { return View(); }
        public ActionResult TaskFiguresReport() { return View(); }
        public ActionResult PersonTasksReport() { return View(); }
        public ActionResult TaskTracking(int? personId, int? taskStatusId) { ViewBag.PersonId = personId; ViewBag.TaskStatusId = taskStatusId; return View(); }
        public ActionResult WorkFlow() { return View(); }
        public ActionResult UnitProject() { return View(); }
        public ActionResult NewOrEdit(int? id)
        {
            Session.Abandon();
            if (id == null)
            {
                var t = new TaskSingleDto();
                t.Task = new Task();
                t.MediaIds = new System.Collections.Generic.List<long>();
                return View(t);
            }
            var result = _workService.GetTaskById(id != null ? id.Value : 0);
            if (result == null)
                Response.Redirect("/Work");
            return View(id != null ? result : new TaskSingleDto());
        }
        public ActionResult GetPageableTasks(int pageSize, int pageNumber, int? assigned, int? requestedBy, int? unitId, int? projectId, int? taskId, int? taskStatusId, string title)
        {
            return Json(_workService.GetPageableTasks(pageSize, pageNumber, assigned, requestedBy, unitId, projectId, taskId, taskStatusId, title), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTasksByAssignedMe()
        {
            return Json(_workService.GetTasksByAssignedMe(Accesses.PersonId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetWorkFlow(int? assigned, int? requestedBy, int? unitId)
        {
            return Json(_workService.GetWorkFlow(assigned, requestedBy, unitId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFilterableTasks(int? assigned, int? requestedBy, int? unitId, int? projectId, int? taskId, int? taskStatusId, string title)
        {
            return Json(_workService.GetFilterableTasks(assigned, requestedBy, unitId, projectId, taskId, taskStatusId, title), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTaskComments(int taskId)
        {
            return Json(_workService.GetTaskComments(taskId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTaskHistory(int taskId)
        {
            return Json(_workService.GetTaskHistory(taskId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProjectsPersonsUnits()
        {
            return Json(new
            {
                Projects = _workService.GetProjects(),
                Persons = _personService.GetPersons(),
                Units = _workService.GetUnits()
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPersonsDaireMudurlukUnits()
        {
            return Json(new
            {
                Persons = _personService.GetPersons(),
                Units = _workService.GetDaireMudurlukUnits()
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDaireMudurlukUnits()
        {
            return Json(_workService.GetDaireMudurlukUnits(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnitProjects(int? unitId)
        {
            return Json(_workService.GetUnitProjects(unitId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnitProjectList(int unitId)
        {
            return Json(_workService.GetUnitProjectList(unitId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUnitPersons(int unitId)
        {
            return Json(_workService.GetUnitPersons(unitId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertTask(TaskParamDto model)
        {
            if (model.TaskStatusId == TaskStatusTypes.ToDo && !string.IsNullOrEmpty(model.ResultContent))
                return AjaxMessage("Uyarı", "Sonuç girilen bir işin <b>İş Durumu Alanı</b> Yapılacak olarak bırakılamaz.", MessageTypes.warning);
            if ((model.TaskStatusId == TaskStatusTypes.Done || model.TaskStatusId == TaskStatusTypes.Removed) && model.DueDate == null)
                return AjaxMessage("Uyarı", "İş durumu tamamlandı ya da kaldırıldı olan bir işin <b>Kapanma Tarihi</b> alanı zorunludur.", MessageTypes.warning);
            //if ((model.TaskStatusId == TaskStatusTypes.Removed || model.TaskStatusId == TaskStatusTypes.Done) && string.IsNullOrEmpty(model.ResultContent))
            //    return AjaxMessage("Uyarı", "Kaldırılan veya Tamamlanan bir işin <b>Sonuç</b> Alanı zorunludur.", MessageTypes.warning);
            try
            {
                var personId = Accesses.PersonId;

                _uow.BeginTransaction();
                var entity = new Task
                {
                    Assigned = model.Assigned,
                    CreatedBy = personId,
                    CreatedDate = DateTime.Now,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    ProjectId = model.ProjectId,
                    RequestedBy = model.RequestedBy,
                    RequestedDate = model.RequestedDate,
                    ResultContent = model.ResultContent,
                    TaskStatusId = model.TaskStatusId,
                    Title = model.Title,
                    UnitId = model.UnitId,
                    Queue = model.Queue,
                    Followers = model.Followers
                };
                entity.DueDate = (model.TaskStatusId != TaskStatusTypes.ToDo && model.TaskStatusId != TaskStatusTypes.InProgress) ? model.DueDate : null;

                _workService.InsertTask(entity);
                _uow.SaveChanges();

                var entityTaskHistory = new TaskHistory
                {
                    CreatedBy = personId,
                    CreatedDate = DateTime.Now,
                    Assigned = entity.Assigned,
                    TaskId = entity.Id,
                    TaskStatusId = model.TaskStatusId,
                };

                _workService.InsertTaskHistory(entityTaskHistory);
                _uow.SaveChanges();

                var entityMedia = (Media)Session["Media" + 1];

                if (entityMedia != null)
                {
                    var counter = int.Parse(Session["counter"].ToString());
                    for (int i = 1; i <= counter; i++)
                    {
                        var entityM = (Media)Session["Media" + i];
                        entityM.CreatedDate = DateTime.Now;
                        entityM.MediaTypeId = MediaTypes.IsYonetimi;
                        _mediaService.InsertMedia(entityM);
                        _uow.SaveChanges();

                        _mediaService.InsertTaskMedia(new TaskMedia
                        {
                            CreatedBy = personId,
                            CreatedDate = DateTime.Now,
                            MediaId = entityM.Id,
                            TaskId = entity.Id
                        });
                        _uow.SaveChanges();
                    }
                }

                _uow.Commit();


                if (bool.Parse(Resources.Parameter.IsSendMailForWork))
                {
                    try
                    {
                        if (model.TaskStatusId == TaskStatusTypes.Done)
                        {
                            var requestedBy = _personService.FindPerson(entity.RequestedBy);
                            var assigned = _personService.FindPerson(entity.Assigned);
                            var ccList = model.Followers != null ? model.Followers.Split(',').ToList().Select(p => _personService.FindPerson(int.Parse(p)).Email).Where(p => string.IsNullOrEmpty(p) == false).ToList() : new List<string>();
                            if (assigned.Email != null)
                                ccList.Add(assigned.Email);

                            if (requestedBy.Email != null)
                            {
                                var sr = new StreamReader(Server.MapPath(@"/Views/Shared/EmailTemplate/") + "TaskDoneEmail.html");
                                string body = sr.ReadToEnd();

                                var result = _workService.GetTaskByTaskId(entity.Id);
                                var folowerNames = _workService.GetTaskFolowerNames(entity.Id);

                                string messageBody = body.Replace("##Assigned", result[0].Assigned).Replace("##RequestedBy", result[0].RequestedBy)
                                    .Replace("##taskId", entity.Id.ToString()).Replace("##RequestedDate", result[0].RequestedDate).Replace("##ProjectName", result[0].ProjectName)
                                    .Replace("##Title", result[0].Title).Replace("##Description", result[0].Description != null ? result[0].Description.Replace("\n", "<br/>") : "").Replace("##CreatedDate", result[0].CreatedDate)
                                    .Replace("##DueDate", result[0].DueDate).Replace("##ResultContent", result[0].ResultContent != null ? result[0].ResultContent.Replace("\n", "<br/>") : "")
                                    .Replace("##Followers", folowerNames);
                                var attachementList = new List<Attachment>();
                                if (Session["counter"] != null)
                                {
                                    var counter = int.Parse(Session["counter"].ToString());
                                    for (int i = 1; i <= counter; i++)
                                    {
                                        var entityM = (Media)Session["Media" + i];
                                        attachementList.Add(new Attachment(new MemoryStream(entityM.Value), entityM.FileName, entityM.ContentType));
                                    }
                                }
                                MailHelper.SendMail("Kapanan İş - " + result[0].Title, messageBody, requestedBy.Email, ccList.Distinct().ToList(), attachementList);
                            }
                        }
                        else
                        {
                            var requestedBy = _personService.FindPerson(entity.RequestedBy);
                            var assigned = _personService.FindPerson(entity.Assigned);
                            var ccList = model.Followers != null ? model.Followers.Split(',').ToList().Select(p => _personService.FindPerson(int.Parse(p)).Email).Where(p => string.IsNullOrEmpty(p) == false).ToList() : new List<string>();
                            if (requestedBy.Email != null)
                                ccList.Add(requestedBy.Email);

                            if (assigned.Email != null)
                            {
                                var sr = new StreamReader(Server.MapPath(@"/Views/Shared/EmailTemplate/") + "TaskDetailEmail.html");
                                string body = sr.ReadToEnd();

                                var result = _workService.GetTaskByTaskId(entity.Id);
                                var folowerNames = _workService.GetTaskFolowerNames(entity.Id);

                                string messageBody = body.Replace("##Assigned", result[0].Assigned).Replace("##RequestedBy", result[0].RequestedBy)
                                    .Replace("##taskId", entity.Id.ToString()).Replace("##RequestedDate", result[0].RequestedDate).Replace("##ProjectName", result[0].ProjectName)
                                    .Replace("##Title", result[0].Title).Replace("##Description", result[0].Description != null ? result[0].Description.Replace("\n", "<br/>") : "")
                                    .Replace("##CreatedBy", result[0].CreatedBy).Replace("##CreatedDate", result[0].CreatedDate).Replace("##Followers", folowerNames);

                                var attachementList = new List<Attachment>();
                                if (entityMedia != null)
                                {
                                    var counter = int.Parse(Session["counter"].ToString());
                                    for (int i = 1; i <= counter; i++)
                                    {
                                        var entityM = (Media)Session["Media" + i];
                                        attachementList.Add(new Attachment(new MemoryStream(entityM.Value), entityM.FileName, entityM.ContentType));
                                    }
                                }
                                MailHelper.SendMail("Yeni İş - " + result[0].Title, messageBody, assigned.Email, ccList.Distinct().ToList(), attachementList);
                            }
                        }

                    }
                    catch (Exception ex) { }
                }

                return AjaxMessage("", "İşlem başarıyla gerçekleşti.$$window.location='/Work/NewOrEdit?Id=" + entity.Id + "'", MessageTypes.funcAndMessage);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                throw ex;
            }
        }
        public ActionResult UpdateTask(TaskParamDto model)
        {
            if (model.TaskStatusId == TaskStatusTypes.ToDo && !string.IsNullOrEmpty(model.ResultContent))
                return AjaxMessage("Uyarı", "Sonuç girilen bir işin <b>İş Durumu</b> Alanı Yapılacak olarak bırakılamaz.", MessageTypes.warning);
            //if ((model.TaskStatusId == TaskStatusTypes.Removed || model.TaskStatusId == TaskStatusTypes.Done) && string.IsNullOrEmpty(model.ResultContent))
            //    return AjaxMessage("Uyarı", "Kaldırılan veya Tamamlanan bir işin <b>Sonuç</b> Alanı zorunludur.", MessageTypes.warning);
            if ((model.TaskStatusId == TaskStatusTypes.Removed || model.TaskStatusId == TaskStatusTypes.Done) && model.DueDate == null)
                return AjaxMessage("Uyarı", "Kaldırılan veya Tamamlanan bir işin <b>Kapanma Tarihi</b> Alanı zorunludur.", MessageTypes.warning);
            var entity = _workService.FindTask(model.Id);
            var entityTaskStatusId = entity.TaskStatusId;
            var entityAssigned = entity.Assigned;
            try
            {
                var personId = Accesses.PersonId;
                _uow.BeginTransaction();

                if (entity.TaskStatusId != model.TaskStatusId | entity.Assigned != model.Assigned)
                {
                    var entityTaskHistory = new TaskHistory
                    {
                        CreatedBy = personId,
                        CreatedDate = DateTime.Now,
                        Assigned = model.Assigned,
                        TaskId = entity.Id,
                        TaskStatusId = model.TaskStatusId,
                    };
                    _workService.InsertTaskHistory(entityTaskHistory);
                    _uow.SaveChanges();
                }

                entity.Assigned = model.Assigned;
                entity.Description = model.Description;
                entity.DueDate = model.DueDate;
                entity.ProjectId = model.ProjectId;
                entity.RequestedBy = model.RequestedBy;
                entity.RequestedDate = model.RequestedDate;
                entity.ResultContent = model.ResultContent;
                entity.TaskStatusId = model.TaskStatusId;
                entity.Title = model.Title;
                entity.DueDate = (model.TaskStatusId == TaskStatusTypes.Removed || model.TaskStatusId == TaskStatusTypes.Done) ? model.DueDate : null;
                entity.UnitId = model.UnitId;
                entity.Queue = model.Queue;
                entity.Followers = model.Followers;

                _workService.UpdateTask(entity);
                _uow.SaveChanges();

                var entityMedia = (Media)Session["Media" + 1];
                if (entityMedia != null | Session["Clear"] == null)
                {
                    var resultList = _mediaService.GetTaskMediaById(entity.Id);

                    for (int i = 0; i < resultList.Count; i++)
                    {
                        _mediaService.DeleteTaskMedia(resultList[i]);
                        _uow.SaveChanges();

                        var entityM = _mediaService.FindMedia(resultList[i].MediaId);
                        _mediaService.DeleteMedia(entityM);
                        _uow.SaveChanges();
                    }

                    if (Session["counter"] != null)
                    {
                        var counter = int.Parse(Session["counter"].ToString());

                        for (int i = 1; i <= counter; i++)
                        {
                            var entityM = (Media)Session["Media" + i];
                            entityM.CreatedDate = DateTime.Now;
                            entityM.MediaTypeId = MediaTypes.IsYonetimi;
                            _mediaService.InsertMedia(entityM);
                            _uow.SaveChanges();

                            _mediaService.InsertTaskMedia(new TaskMedia
                            {
                                CreatedBy = Accesses.PersonId,
                                CreatedDate = DateTime.Now,
                                MediaId = entityM.Id,
                                TaskId = entity.Id
                            });
                            _uow.SaveChanges();
                        }
                    }
                }

                _uow.Commit();


                if (bool.Parse(Resources.Parameter.IsSendMailForWork))
                {
                    try
                    {
                        if (model.TaskStatusId == TaskStatusTypes.Done && model.TaskStatusId != entityTaskStatusId)
                        {
                            var requestedBy = _personService.FindPerson(entity.RequestedBy);
                            var assigned = _personService.FindPerson(entity.Assigned);
                            var ccList = model.Followers != null ? model.Followers.Split(',').ToList().Select(p => _personService.FindPerson(int.Parse(p)).Email).Where(p => string.IsNullOrEmpty(p) == false).ToList() : new List<string>();
                            if (assigned.Email != null)
                                ccList.Add(assigned.Email);

                            if (requestedBy.Email != null)
                            {
                                var sr = new StreamReader(Server.MapPath(@"/Views/Shared/EmailTemplate/") + "TaskDoneEmail.html");
                                string body = sr.ReadToEnd();

                                var result = _workService.GetTaskByTaskId(entity.Id);
                                var folowerNames = _workService.GetTaskFolowerNames(entity.Id);

                                string messageBody = body.Replace("##Assigned", result[0].Assigned).Replace("##RequestedBy", result[0].RequestedBy)
                                    .Replace("##taskId", entity.Id.ToString()).Replace("##RequestedDate", result[0].RequestedDate).Replace("##ProjectName", result[0].ProjectName)
                                    .Replace("##Title", result[0].Title).Replace("##Description", result[0].Description != null ? result[0].Description.Replace("\n", "<br/>") : "").Replace("##CreatedDate", result[0].CreatedDate)
                                    .Replace("##DueDate", result[0].DueDate).Replace("##ResultContent", result[0].ResultContent != null ? result[0].ResultContent.Replace("\n", "<br/>") : "")
                                    .Replace("##Followers", folowerNames);

                                var attachementList = new List<Attachment>();
                                if (Session["counter"] != null)
                                {
                                    var counter = int.Parse(Session["counter"].ToString());
                                    for (int i = 1; i <= counter; i++)
                                    {
                                        var entityM = (Media)Session["Media" + i];
                                        attachementList.Add(new Attachment(new MemoryStream(entityM.Value), entityM.FileName, entityM.ContentType));
                                    }
                                }
                                MailHelper.SendMail("Kapanan İş - " + result[0].Title, messageBody, requestedBy.Email, ccList.Distinct().ToList(), attachementList);
                            }
                        }
                        else
                            if (entityAssigned != model.Assigned)
                            {
                                var requestedBy = _personService.FindPerson(entity.RequestedBy);
                                var assigned = _personService.FindPerson(entity.Assigned);
                                var ccList = model.Followers != null ? model.Followers.Split(',').ToList().Select(p => _personService.FindPerson(int.Parse(p)).Email).Where(p => string.IsNullOrEmpty(p) == false).ToList() : new List<string>();
                                if (requestedBy.Email != null)
                                    ccList.Add(requestedBy.Email);

                                if (assigned.Email != null)
                                {
                                    var sr = new StreamReader(Server.MapPath(@"/Views/Shared/EmailTemplate/") + "TaskDetailEmail.html");
                                    string body = sr.ReadToEnd();

                                    var result = _workService.GetTaskByTaskId(entity.Id);
                                    var folowerNames = _workService.GetTaskFolowerNames(entity.Id);

                                    string messageBody = body.Replace("##Assigned", result[0].Assigned).Replace("##RequestedBy", result[0].RequestedBy)
                                        .Replace("##taskId", entity.Id.ToString()).Replace("##RequestedDate", result[0].RequestedDate).Replace("##ProjectName", result[0].ProjectName)
                                        .Replace("##Title", result[0].Title).Replace("##Description", result[0].Description != null ? result[0].Description.Replace("\n", "<br/>") : "")
                                        .Replace("##CreatedBy", result[0].CreatedBy).Replace("##CreatedDate", result[0].CreatedDate).Replace("##Followers", folowerNames);

                                    var attachementList = new List<Attachment>();
                                    if (Session["counter"] != null)
                                    {
                                        var counter = int.Parse(Session["counter"].ToString());
                                        for (int i = 1; i <= counter; i++)
                                        {
                                            var entityM = (Media)Session["Media" + i];
                                            attachementList.Add(new Attachment(new MemoryStream(entityM.Value), entityM.FileName, entityM.ContentType));
                                        }
                                    }
                                    MailHelper.SendMail("Yeni İş - " + result[0].Title, messageBody, assigned.Email, ccList.Distinct().ToList(), attachementList);
                                }
                            }
                    }
                    catch (Exception ex) { }
                }
                return AjaxMessage("", "İşlem başarıyla gerçekleşti.$$window.location='/Work/NewOrEdit?Id=" + entity.Id + "'", MessageTypes.funcAndMessage);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult UpdateTaskQueue(int taskId, int queue)
        {
            var entity = _workService.FindTask(taskId);
            var personId = Accesses.PersonId;
            entity.Queue = queue;
            _workService.UpdateTask(entity);
            _uow.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public ActionResult InsertProject(string Name, int UnitId)
        {
            if (string.IsNullOrEmpty(Name) == false && UnitId != 0)
            {
                var entity = new Project
                {
                    Name = Name,
                    CreatedBy = Accesses.PersonId,
                    CreatedDate = DateTime.Now,
                    UnitId = UnitId
                };
                _workService.InsertProject(entity);
                _uow.SaveChanges();

                return AjaxSuccess();
            }
            return null;
        }
        [HttpPost]
        public ActionResult UpdateProject(string Name, int ProjectId)
        {
            if (string.IsNullOrEmpty(Name) == false && ProjectId != 0)
            {
                var entity = _workService.FindProject(ProjectId);
                entity.Name = Name;
                _workService.UpdateProject(entity);
                _uow.SaveChanges();
                return AjaxSuccess();
            }
            return null;
        }
        [HttpPost]
        public ActionResult InsertTaskComment(Comment model)
        {
            if (string.IsNullOrEmpty(model.Description))
                return AjaxMessage("Uyarı", "Yorum alanı gereklidir.", MessageTypes.warning);
            var entity = new Comment
            {
                CommentedBy = Accesses.PersonId,
                CommentedDate = DateTime.Now,
                Description = model.Description,
                TaskId = model.TaskId
            };
            _workService.InsertTaskComment(entity);
            _uow.SaveChanges();

            if (bool.Parse(Resources.Parameter.IsSendMailForWork))
            {
                try
                {
                    var entityTask = _workService.FindTask(model.TaskId);
                    var assigned = _personService.FindPerson(entityTask.Assigned);
                    var requestedBy = _personService.FindPerson(entityTask.RequestedBy);
                    var ccList = entityTask.Followers == null ? new List<string>() : entityTask.Followers.Split(',').ToList().Select(p => _personService.FindPerson(int.Parse(p)).Email).Where(p => string.IsNullOrEmpty(p) == false).ToList();
                    if (assigned.Email != null)
                        ccList.Add(Accesses.Email);
                    if (requestedBy.Email != null)
                        ccList.Add(requestedBy.Email);

                    var sr = new StreamReader(Server.MapPath(@"/Views/Shared/EmailTemplate/") + "TaskComment.html");
                    string body = sr.ReadToEnd();
                    var result = _workService.GetTaskByTaskId(entityTask.Id);

                    string messageBody = body.Replace("##CommentedBy", Accesses.FullName).Replace("##taskId", entityTask.Id.ToString())
                        .Replace("##Description", model.Description != null ? model.Description.Replace("\n", "<br/>") : "");
                    var attachementList = new List<Attachment>();
                    MailHelper.SendMail("Yeni Yorum - " + result[0].Title, messageBody, assigned.Email, ccList.Distinct().ToList(), attachementList);

                }
                catch (Exception ex) { throw; }
            }
            return Json("");
        }
        [HttpPost]
        public ActionResult InsertTaskHistory(TaskHistoryParamDto model)
        {
            var entity = _workService.FindTask(model.TaskId);
            try
            {
                if (entity.TaskStatusId != model.TaskStatusId)
                {
                    var personId = Accesses.PersonId;
                    _uow.BeginTransaction();

                    entity.TaskStatusId = model.TaskStatusId;
                    entity.DueDate = (model.TaskStatusId != TaskStatusTypes.ToDo && model.TaskStatusId != TaskStatusTypes.InProgress) ? (DateTime?)DateTime.Now : null;

                    _workService.UpdateTask(entity);
                    _uow.SaveChanges();

                    var entityTaskHistory = new TaskHistory
                    {
                        CreatedBy = personId,
                        CreatedDate = DateTime.Now,
                        Assigned = entity.Assigned,
                        TaskId = entity.Id,
                        TaskStatusId = entity.TaskStatusId,
                    };
                    _workService.InsertTaskHistory(entityTaskHistory);
                    _uow.SaveChanges();
                    _uow.Commit();
                    if (bool.Parse(Resources.Parameter.IsSendMailForWork))
                    {
                        try
                        {
                            if (model.TaskStatusId == TaskStatusTypes.Done)
                            {
                                var requestedBy = _personService.FindPerson(entity.RequestedBy);
                                var assigned = _personService.FindPerson(entity.Assigned);
                                var ccList = entity.Followers != null ? entity.Followers.Split(',').ToList().Select(p => _personService.FindPerson(int.Parse(p)).Email).Where(p => string.IsNullOrEmpty(p) == false).ToList() : new List<string>();
                                if (assigned.Email != null)
                                    ccList.Add(assigned.Email);

                                if (requestedBy.Email != null)
                                {
                                    var sr = new StreamReader(Server.MapPath(@"/Views/Shared/EmailTemplate/") + "TaskDoneEmail.html");
                                    string body = sr.ReadToEnd();

                                    var result = _workService.GetTaskByTaskId(entity.Id);
                                    var folowerNames = _workService.GetTaskFolowerNames(entity.Id);

                                    string messageBody = body.Replace("##Assigned", result[0].Assigned).Replace("##RequestedBy", result[0].RequestedBy)
                                        .Replace("##taskId", entity.Id.ToString()).Replace("##RequestedDate", result[0].RequestedDate).Replace("##ProjectName", result[0].ProjectName)
                                        .Replace("##Title", result[0].Title).Replace("##Description", result[0].Description != null ? result[0].Description.Replace("\n", "<br/>") : "").Replace("##CreatedDate", result[0].CreatedDate)
                                        .Replace("##DueDate", result[0].DueDate).Replace("##ResultContent", result[0].ResultContent != null ? result[0].ResultContent.Replace("\n", "<br/>") : "")
                                        .Replace("##Followers", folowerNames);

                                    var attachementList = new List<Attachment>();
                                    MailHelper.SendMail("Kapanan İş - " + result[0].Title, messageBody, requestedBy.Email, ccList.Distinct().ToList(), attachementList);
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return Json(false);
                throw ex;
            }
        }
        public ActionResult GetTaskStatusCounts()
        {
            return Json(_workService.GetTaskStatusCounts(Accesses.UnitId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPersonTasktStatusCounts(DateTime? startDate, DateTime? endDate)
        {
            return Json(_workService.GetPersonTasktStatusCounts(startDate, endDate, Accesses.UnitId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPersonTaskList(int? projectId, int? unitId, int? personId, int? taskStatusId, DateTime? startDate, DateTime? endDate)
        {
            return Json(_workService.GetPersonTaskList(projectId, unitId, personId, taskStatusId, startDate, endDate), JsonRequestBehavior.AllowGet);
        }
    }
}