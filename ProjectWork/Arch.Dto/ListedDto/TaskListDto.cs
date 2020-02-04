namespace Arch.Dto.ListedDto
{
    public class TaskListDto
    {
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public int Queue { get; set; }
        public short TaskStatusId { get; set; }
        public string ProjectName { get; set; }
        public string RequestedBy { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RequestedDate { get; set; }
        public string Assigned { get; set; }
        public string TaskStatusName { get; set; }
        public string DueDate { get; set; }
        public string ResultContent { get; set; }
        public string CreatedDate { get; set; }
        public string MediaIds { get; set; }
        public string Initials { get; set; }
        public int ResultCount { get; set; }
    }
}