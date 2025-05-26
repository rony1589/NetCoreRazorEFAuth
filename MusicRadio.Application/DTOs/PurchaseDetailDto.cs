using MusicRadio.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Application.DTOs
{
    public class PurchaseDetailDto
    {
        public int Id { get; set; }
        public string Client_Id { get; set; } = string.Empty;
        public int Album_Id { get; set; }
        public decimal Total { get; set; }

        public AlbumSet? AlbumSet { get; set; } = null!;

        public Client? Client { get; set; } = null!;

        public IList<SongSet>? SongSet { get; set; } = null!;

    }
}
