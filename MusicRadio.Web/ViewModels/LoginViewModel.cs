using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicRadio.Web.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(50, ErrorMessage = "El tamaño máximo son 50 caracteres")]
        [DisplayName("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(30, ErrorMessage = "El tamaño máximo son 30 caracteres")]
        [DisplayName("Contraseña")]
        public string Password { get; set; } = string.Empty;
    }
}
