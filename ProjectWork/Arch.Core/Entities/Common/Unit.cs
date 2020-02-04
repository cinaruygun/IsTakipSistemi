namespace Arch.Core
{
    public partial class Unit : BaseEntity
    {
        public int UnitTypeId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public int? YbsUnitId { get; set; }
        public int? PdksUnitId { get; set; }
    }
}