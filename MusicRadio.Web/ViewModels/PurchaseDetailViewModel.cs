using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MusicRadio.Web.ViewModels
{
    public class PurchaseDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Cliente obligatorio")]
        [DisplayName("Cliente")]
        public string Client_Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un Album")]
        [DisplayName("Album")]
        public int Album_Id { get; set; }

        [Required(ErrorMessage = "El Total es obligatorio")]
        [DisplayName("Total")]
        public decimal Total { get; set; }

        public AlbumSet? AlbumSet { get; set; } = null!;

        public IList<SongSetDto>? SongSet { get; set; } = null!;

        public Client? Client { get; set; } = null!;
    }
}
