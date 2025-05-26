using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Interfaces
{
    public interface ISongSetService
    {
        Task<IEnumerable<SongSet>> GetAllAsync();
        Task<SongSet?> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(SongSet song);
        Task<OperationResult> UpdateAsync(SongSet song);
        Task<OperationResult> DeleteAsync(int id);
    }
}
