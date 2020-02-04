using Arch.Core;
using Arch.Dto.ListedDto;
using System.Collections.Generic;
namespace Arch.Service.Interfaces
{
    public interface IMediaService
    {
        Media FindMedia(long id);
        void InsertMedia(Media entity);
        void UpdateMedia(Media entity);
        void DeleteMedia(Media entity);
        void InsertTaskMedia(TaskMedia entity);
        List<TaskMedia> GetTaskMediaById(int taskId);
        void DeleteTaskMedia(TaskMedia entity);
    }
}
