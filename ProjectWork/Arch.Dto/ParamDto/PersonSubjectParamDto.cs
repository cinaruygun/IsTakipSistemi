using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Arch.Dto.ParamDto
{
    public partial class PersonSubjectParamDto
    {
        [Required]
        [DisplayName("Kiþi")]
        public int PersonId { get; set; }
        [Required]
        [DisplayName("Etkinlik Konusu")]
        public List<short> SubjectId { get; set; }
        [Required]
        [DisplayName("Profesyonel Unvan")]
        public string ProTitle { get; set; }
        public bool IsInstructor { get; set; }
    }
}