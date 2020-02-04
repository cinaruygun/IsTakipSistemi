using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Arch.Dto.ParamDto
{
    public partial class RequestTaskParamDto
    {
        [Required]
        public int Id { get; set; }
        [DisplayName("Talep Edilen Birim")]
        [Range(1, int.MaxValue, ErrorMessage = "Talep Edilen Birim alan� gereklidir.")]
        public int UnitId { get; set; }
        [Required]
        [DisplayName("Ba�l�k")]
        public string Title { get; set; }
        public string Followers { get; set; }
        public string Description { get; set; }
    }
}
