using System;
namespace Arch.Service.Interfaces
{
    public interface IUtilityService
    {
        bool AnyEmail(string email);
        System.Collections.Generic.List<Arch.Core.LookupList> GetLookupLists();
        System.Collections.Generic.List<Arch.Core.Parameters> GetParameters();
        int GetTaskCount(int? assigned);
        int? GetUnitFirstProjectId(int unitId);
    }
}
