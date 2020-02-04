using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Arch.Dto.ParamDto
{
    public partial class SubjectParamDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Konu Adý")]
        public string Name { get; set; }
    }
}