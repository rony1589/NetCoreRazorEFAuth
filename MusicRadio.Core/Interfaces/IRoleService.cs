using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(Role role);
        Task<OperationResult> UpdateAsync(Role role);
        Task<OperationResult> DeleteAsync(int id);
    }
}
