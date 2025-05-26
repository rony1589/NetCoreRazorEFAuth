using MusicRadio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(Client client);
    }
}
