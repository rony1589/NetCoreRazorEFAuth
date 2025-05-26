using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Entities
{
    public class PurchaseDetail
    {
        public int Id { get; set; }

        public string Client_Id { get; set; } = string.Empty;

        public int Album_Id { get; set; }
        public decimal Total { get; set; }

        public AlbumSet? AlbumSet { get; set; } = null!;

        public Client? Client { get; set; } = null!;


    }
}
