using Arch.Core;
using Arch.Dto.ListedDto;
using Arch.Dto.SingleDto;
using System.Collections.Generic;
namespace Arch.Service.Interfaces
{
    public interface IPersonService
    {
        Person GetPerson(string userName, string password);
        Person FindPerson(int id);
        List<System.Web.UI.WebControls.ListItem> SearchPerson(string word);
        object GetPersons();
        List<string> GetPersonMails(List<int> ids);
    }
}
