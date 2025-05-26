using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Entities
{
    public class AlbumSet
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Decimal Precio { get; set; }
        public ICollection<PurchaseDetail>? PurchaseDetails { get; set; } = null!;
        public ICollection<SongSet>? SongSets { get; set; } = null!;
    }
}
