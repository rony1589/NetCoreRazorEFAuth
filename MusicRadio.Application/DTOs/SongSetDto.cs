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
    public  class SongSetDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Album_Id { get; set; }

        public AlbumSet? AlbumSet { get; set; } = null!;

    }
}
