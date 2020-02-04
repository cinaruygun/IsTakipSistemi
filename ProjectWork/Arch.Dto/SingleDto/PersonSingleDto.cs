namespace Arch.Dto.SingleDto
{
    public class PersonSingleDto
    {
        public int PersonId { get; set; }
        public int UnitId { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? ImageId { get; set; }
    }
}