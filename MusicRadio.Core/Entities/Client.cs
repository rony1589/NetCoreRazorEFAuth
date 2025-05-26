using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Entities
{
    public class Client
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Role_Id { get; set; }

        public Role Role { get; set; } = null!;
        public ICollection<PurchaseDetail> PurchaseDetails { get; set; } = null!;
    }
}
