using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Application.DTOs
{
    public class ClientDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Mail { get; set; } = string.Empty;

        public string Direction { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role_Id { get; set; } = UserRole.Cliente;
       
    }
}
