using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Entities
{
    public class SongSet
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Album_Id { get; set; }

        public AlbumSet? AlbumSet { get; set; } = null!;

    }
}
