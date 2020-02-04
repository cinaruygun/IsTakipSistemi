using Arch.Core;
using Arch.Core.Constants;
using Arch.Core.Enums;
using Arch.Data.UnitofWork;
using Arch.Dto.ListedDto;
using Arch.Dto.SingleDto;
using Arch.Service.Abstracts;
using Arch.Service.Interfaces;
using Arch.Utilities.Manager;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
namespace Arch.Service.Services
{
    public class PersonService : APersonService, IPersonService
    {
        public PersonService(IUnitofWork uow) : base(uow) { }
        public Person FindPerson(int id)
        {
            return _personRepository.Find(id);
        }
        public Person GetPerson(string userName, string password)
        {
            var result = _personRepository.GetAll().Where(p => p.UserName == userName).SingleOrDefault();
            if (result == null) return null;
            if (EnDeCode.Decrypt(result.Password, StaticParams.SifrelemeParametresi) == password)
                return result;
            else
                return null;
        }
        public List<string> GetPersonMails(List<int> ids)
        {
            return _personRepository.GetAll().Where(p => ids.Contains(p.Id)).Select(p => p.Email).ToList();
        }
        public List<System.Web.UI.WebControls.ListItem> SearchPerson(string word)
        {
            var result = _personRepository.GetAll().Where(p => (p.Email.Contains(word) ||
                (p.Name + " " + p.Surname).Contains(word))).Take(5).ToList().Select(p => new System.Web.UI.WebControls.ListItem
                {
                    Text = p.Email + " [" + p.Name + " " + p.Surname + "]",
                    Value = p.Id.ToString()
                }).ToList();
            return result;
        }
        public object GetPersons()
        {
            return _personRepository.GetAll().Select(p => new { id = p.Id, text = p.Name + " " + p.Surname }).OrderBy(p => p.text).ToList();
        }
    }
}