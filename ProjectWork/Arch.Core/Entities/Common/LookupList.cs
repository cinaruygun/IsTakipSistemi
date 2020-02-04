namespace Arch.Core
{
    public partial class LookupList
    {
        public short Id { get; set; }
        public short? ParentId { get; set; }
        public byte LookupId { get; set; }
        public string Name { get; set; }
        public string LongName { get; set; }
        public bool IsActive { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public string Value { get; set; }
    }
}