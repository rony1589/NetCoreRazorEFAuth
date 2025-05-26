using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Application.DTOs
{
    public class RoleDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El tamaño máximo son 50 caracteres")]
        [DisplayName("Nombre del Rol")]
        public string Name { get; set; } = string.Empty;
    }
}
