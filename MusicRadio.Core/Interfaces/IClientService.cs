using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(string id);
        Task<OperationResult> AddAsync(Client client);
        Task<OperationResult> UpdateAsync(Client client);
        Task<OperationResult> DeleteAsync(string id);
    }
}
