namespace Arch.Core
{
    using System.ComponentModel.DataAnnotations;
    public partial class Media
    {
        [Key]
        public long Id { get; set; }
        public short MediaTypeId { get; set; }
        public byte[] Value { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int ContentLength { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}