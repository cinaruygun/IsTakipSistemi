using System.ComponentModel.DataAnnotations;
namespace Arch.Core
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}