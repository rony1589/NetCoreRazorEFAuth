using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MusicRadio.Web.ViewModels
{
    public class AlbumViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre del álbum es obligatorio")]
        [MaxLength(100, ErrorMessage = "El tamaño máximo son 100 caracteres")]
        [DisplayName("Nombre del álbum")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "El precio del álbum es obligatorio")]
        [DisplayName("Precio del álbum")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.01")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio solo puede tener hasta 2 decimales")]
        [DataType(DataType.Currency)] // Indicar que es un valor monetario
        public Decimal Precio { get; set; }

    }
}
