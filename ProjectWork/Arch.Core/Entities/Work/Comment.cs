namespace Arch.Core
{
    public partial class Comment : BaseEntity
    {
        public int TaskId { get; set; }
        public string Description { get; set; }
        public System.DateTime CommentedDate { get; set; }
        public int CommentedBy { get; set; }
    }
}
