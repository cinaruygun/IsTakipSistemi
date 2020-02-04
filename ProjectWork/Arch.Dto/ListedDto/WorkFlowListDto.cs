namespace Arch.Dto.ListedDto
{
    public class WorkFlowListDto
    {
        public int TaskId { get; set; }
        public short TaskStatusId { get; set; }
        public string MessageType { get; set; }
        public string Assigned { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public System.DateTime ADate { get; set; }
        public string ADateString { get; set; }
        public string CreatedBy { get; set; }
    }
}