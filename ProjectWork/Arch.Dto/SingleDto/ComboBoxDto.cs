using System.ComponentModel.DataAnnotations.Schema;
namespace Arch.Dto.SingleDto
{
    public class ComboBoxDto
    {
        [Column(TypeName = "varchar")]
        public string Value { get; set; }
        [Column(TypeName = "varchar")]
        public string Text { get; set; }
    }
    public class ComboBoxIdTextDto
    {
        public string id { get; set; }
        public string text { get; set; }
    }
    public class ComboBoxLabelValueDto
    {
        public string label { get; set; }
        public int value { get; set; }
    }
}