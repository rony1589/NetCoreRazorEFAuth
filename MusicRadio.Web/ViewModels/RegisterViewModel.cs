using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MusicRadio.Web.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El número de identificación es obligatorio")]
        [MaxLength(10, ErrorMessage = "El tamaño máximo son 10 caracteres")]
        [DisplayName("Número de identificación")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El tamaño máximo son 100 caracteres")]
        [DisplayName("Nombre completo")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El Email es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo son 50 caracteres")]
        [EmailAddress]
        [DisplayName("Email")]
        public string Mail { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [MaxLength(500, ErrorMessage = "El tamaño máximo son 500 caracteres")]
        [DisplayName("Dirección")]
        public string Direction { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [MaxLength(20, ErrorMessage = "El tamaño máximo son 20 caracteres")]
        [DisplayName("Teléfono")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MaxLength(30, ErrorMessage = "El tamaño máximo son 30 caracteres")]
        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación de contraseña es obligatoria.")]
        [MaxLength(30, ErrorMessage = "El tamaño máximo son 30 caracteres")]
        [DisplayName("Confirmar Contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

}
