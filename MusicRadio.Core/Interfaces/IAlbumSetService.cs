using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Interfaces
{
    public interface IAlbumSetService
    {
        Task<IEnumerable<AlbumSet>> GetAllAsync();
        Task<AlbumSet?> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(AlbumSet album);
        Task<OperationResult> UpdateAsync(AlbumSet album);
        Task<OperationResult> DeleteAsync(int id);
    }

}
