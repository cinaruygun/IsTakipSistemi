using Arch.Core;
using Arch.Data.GenericRepository;
using Arch.Data.UnitofWork;
using Arch.Dto.ListedDto;
using Arch.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
namespace Arch.Service.Services
{
    public class MediaService : IMediaService
    {
        private readonly IGenericRepository<Media> _mediaRepository;
        private readonly IGenericRepository<TaskMedia> _taskMediaRepository;

        public MediaService(IUnitofWork uow)
        {
            _mediaRepository = uow.GetRepository<Media>();
            _taskMediaRepository = uow.GetRepository<TaskMedia>();
        }
        public Media FindMedia(long id)
        {
            return _mediaRepository.GetAll().Where(p => p.Id == id).SingleOrDefault();
        }
        public void InsertMedia(Media entity)
        {
            _mediaRepository.Insert(entity);
        }
        public void UpdateMedia(Media entity)
        {
            _mediaRepository.Update(entity);
        }
        public void DeleteMedia(Media entity)
        {
            _mediaRepository.Delete(entity);
        }
        public List<TaskMedia> GetTaskMediaById(int taskId)
        {
            return _taskMediaRepository.GetAll().Where(p => p.TaskId == taskId).ToList();
        }
        public void InsertTaskMedia(TaskMedia entity)
        {
            _taskMediaRepository.Insert(entity);
        }
        public void DeleteTaskMedia(TaskMedia entity)
        {
            _taskMediaRepository.Delete(entity);
        }
    }
}