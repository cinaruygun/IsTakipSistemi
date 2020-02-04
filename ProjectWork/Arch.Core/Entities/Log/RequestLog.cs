namespace Arch.Core
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class RequestLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public System.Guid Id { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int? PersonId { get; set; }
        public short PermissionId { get; set; }
        public short? MessageTypeId { get; set; }
        public short? MessageContentTypeId { get; set; }
        public string IpAddress { get; set; }
        public string MessageParameters { get; set; }
        public string ActionParameters { get; set; }
        public string BrowserName { get; set; }
        public string MobileDeviceModel { get; set; }
    }
}