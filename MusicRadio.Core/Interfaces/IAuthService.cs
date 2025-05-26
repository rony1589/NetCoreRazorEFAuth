using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult> LoginAsync(string email, string password);

        Task LogoutAsync();
    }
}
