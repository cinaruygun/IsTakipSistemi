using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Arch.Dto.ParamDto
{
    public class LoginParamDto
    {
        [Required]
        [DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Şifre")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}