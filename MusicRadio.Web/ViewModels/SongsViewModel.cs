using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MusicRadio.Core.Entities;

namespace MusicRadio.Web.ViewModels
{
    public class SongsViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre de la canción es obligatoria")]
        [MaxLength(4000, ErrorMessage = "El tamaño máximo son 4000 caracteres")]
        [DisplayName("Nombre de la canción")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "Debe seleccionar un Album.")]
        public int Album_Id { get; set; }

        public AlbumSet? AlbumSet { get; set; } = null!;

    }
}
