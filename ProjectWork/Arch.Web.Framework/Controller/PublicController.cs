using Arch.Service.Interfaces;
using Arch.Data.UnitofWork;
namespace Arch.Web.Controllers
{
    public class PublicController : BaseController
    {
        public PublicController(
            IUnitofWork uow,
            ILogService logService)
            : base(uow, logService)
        { }
    }
}