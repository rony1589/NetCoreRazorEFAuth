using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Core.Interfaces
{
    public interface IPurchaseDetailService
    {
        Task<IEnumerable<PurchaseDetail>> GetAllAsync(string? Client_Id);
        Task<PurchaseDetail?> GetByIdAsync(int id);
        Task<OperationResult> AddAsync(PurchaseDetail purchaseDetail);
        Task<OperationResult> DeleteAsync(int id);
    }
}
