using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Arch.Dto.ParamDto
{
    public partial class TaskParamDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("�nem S�ras�")]
        public int Queue { get; set; }
        [Required]
        [DisplayName("Proje")]
        [Range(1, int.MaxValue, ErrorMessage = "Proje alan� gereklidir.")]
        public int ProjectId { get; set; }
        [Required]
        [DisplayName("�lgili Birim")]
        [Range(1, int.MaxValue, ErrorMessage = "�lgili Birim alan� gereklidir.")]
        public int UnitId { get; set; }
        [Required]
        [DisplayName("Talep Eden")]
        [Range(1, int.MaxValue, ErrorMessage = "Talep Eden alan� gereklidir.")]
        public int RequestedBy { get; set; }
        [Required]
        [DisplayName("Ba�l�k")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [DisplayName("Talep Tarihi")]
        public System.DateTime RequestedDate { get; set; }
        [Required]
        [DisplayName("Atanacak Ki�i")]
        public int Assigned { get; set; }
        [Required]
        [DisplayName("�� Durumu")]
        [Range(1, int.MaxValue, ErrorMessage = "�� Durumu alan� gereklidir.")]
        public short TaskStatusId { get; set; }
        public System.DateTime? DueDate { get; set; }
        public string ResultContent { get; set; }
        public string Followers { get; set; }
    }
}
