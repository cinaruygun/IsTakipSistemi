namespace Arch.Core
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class TempRequestLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public int PersonId { get; set; }
        public string RequestUrl { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}