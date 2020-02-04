namespace Arch.Core
{
    public partial class Parameters
    {
        public byte Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool? IsActive{ get; set; }
    }
}