namespace Arch.Core
{
    public partial class TaskHistory
    {
        public long Id { get; set; }
        public int TaskId { get; set; }
        public int? Assigned { get; set; }
        public short TaskStatusId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
