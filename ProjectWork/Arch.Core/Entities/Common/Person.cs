namespace Arch.Core
{
    public partial class Person : BaseEntity
    {
        public int UnitId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Initials { get; set; }
        public string Email { get; set; }
        public decimal? Phone { get; set; }
        public bool IsActive { get; set; }
        public int? ImageId { get; set; }
    }
}