namespace Arch.Dto.ListedDto
{
    public class PersonTasktStatusCountsListDto
    {
        public int PersonId { get; set; }
        public string Person { get; set; }
        public int? IsAcmaSayisi { get; set; }
        public int? Yapilacak { get; set; }
        public int? DevamEden { get; set; }
        public int? Tamamlanan { get; set; }
        public int? Kaldirilan { get; set; }
        public int? Toplam { get; set; }
    }
}