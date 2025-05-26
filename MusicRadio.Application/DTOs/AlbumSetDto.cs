using MusicRadio.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Application.DTOs
{
    public class AlbumSetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Decimal Precio { get; set; }

    }
}
