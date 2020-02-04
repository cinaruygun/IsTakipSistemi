using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Arch.Dto.ParamDto
{
    public partial class TaskHistoryParamDto
    {
        [Required]
        public int TaskId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Ýþ Durumu alaný gereklidir.")]
        public short TaskStatusId { get; set; }
    }
}
