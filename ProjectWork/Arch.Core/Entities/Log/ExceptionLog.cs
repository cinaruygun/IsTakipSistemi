namespace Arch.Core
{
    using System.ComponentModel.DataAnnotations;
    public partial class ExceptionLog
    {
        [Key]
        public long Id { get; set; }
        public System.Guid? RequestId { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string ExceptionUrl { get; set; }
        public string IpAdress { get; set; }
        public string BrowserInfo { get; set; }
        public int HResult { get; set; }
        public bool? IsForbidden { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? ErrorCount { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
    }
}