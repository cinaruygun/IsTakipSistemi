using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Arch.Dto.ParamDto
{
    public partial class TaskParamDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Önem Sýrasý")]
        public int Queue { get; set; }
        [Required]
        [DisplayName("Proje")]
        [Range(1, int.MaxValue, ErrorMessage = "Proje alaný gereklidir.")]
        public int ProjectId { get; set; }
        [Required]
        [DisplayName("Ýlgili Birim")]
        [Range(1, int.MaxValue, ErrorMessage = "Ýlgili Birim alaný gereklidir.")]
        public int UnitId { get; set; }
        [Required]
        [DisplayName("Talep Eden")]
        [Range(1, int.MaxValue, ErrorMessage = "Talep Eden alaný gereklidir.")]
        public int RequestedBy { get; set; }
        [Required]
        [DisplayName("Baþlýk")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [DisplayName("Talep Tarihi")]
        public System.DateTime RequestedDate { get; set; }
        [Required]
        [DisplayName("Atanacak Kiþi")]
        public int Assigned { get; set; }
        [Required]
        [DisplayName("Ýþ Durumu")]
        [Range(1, int.MaxValue, ErrorMessage = "Ýþ Durumu alaný gereklidir.")]
        public short TaskStatusId { get; set; }
        public System.DateTime? DueDate { get; set; }
        public string ResultContent { get; set; }
        public string Followers { get; set; }
    }
}
