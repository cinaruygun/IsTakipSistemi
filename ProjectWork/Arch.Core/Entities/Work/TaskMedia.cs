using System.ComponentModel.DataAnnotations;
namespace Arch.Core
{
    public partial class TaskMedia
    {
        [Key]
        public long Id { get; set; }
        public long MediaId { get; set; }
        public int TaskId { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}