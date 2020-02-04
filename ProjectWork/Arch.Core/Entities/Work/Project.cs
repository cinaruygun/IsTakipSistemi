namespace Arch.Core
{
    public partial class Project : BaseEntity
    {
        public int UnitId { get; set; }
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
