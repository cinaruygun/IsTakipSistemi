namespace Arch.Core
{
    public partial class Task : BaseEntity
    {
        public int ProjectId { get; set; }
        public int UnitId { get; set; }
        public int RequestedBy { get; set; }
        public int Queue { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime RequestedDate { get; set; }
        public int Assigned { get; set; }
        public short TaskStatusId { get; set; }
        public System.DateTime? DueDate { get; set; }
        public string ResultContent { get; set; }
        public string Followers { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
