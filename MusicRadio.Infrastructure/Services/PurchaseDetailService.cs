using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Infrastructure.Data;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Infrastructure.Services
{
    public class PurchaseDetailService(MusicRadioDbContext _context, IMapper _mapper) : IPurchaseDetailService
    {
        public async Task<IEnumerable<PurchaseDetail>> GetAllAsync(string? Client_Id)
        {
            //return await _context.PurchaseDetails
            //    .Where(e => e.Client_Id == Client_Id)
            //    .Include(e => e.AlbumSet) 
            //    .ThenInclude(album => album!.SongSets) 
            //    .Include(e => e.Client) 
            //    .Select(e => new PurchaseDetail
            //    {
            //        Id = e.Id,
            //        Client_Id = e.Client_Id,
            //        Album_Id = e.Album_Id,
            //        AlbumSet = e.AlbumSet != null ? new AlbumSet
            //        {
            //            Id = e.AlbumSet.Id,
            //            Name = e.AlbumSet.Name,
            //            Precio = e.AlbumSet.Precio
            //        } : null,
            //        Client = e.Client != null ? new Client
            //        {
            //            Id = e.Client_Id,
            //            Name = e.Client.Name,
            //            Mail = e.Client.Mail,
            //            Direction = e.Client.Direction,
            //            Phone = e.Client.Phone
            //        } : null,
            //        SongSet = e.AlbumSet != null ? 
            //        e.AlbumSet.SongSets!.Select(song => new SongSet 
            //        { Id = song.Id
            //        , Name = song.Name
            //        , Album_Id = song.Album_Id 
            //        }).ToList() : new List<SongSet>()
            //    }).ToListAsync();

            return await _context.PurchaseDetails
               .Where(e => e.Client_Id == Client_Id)
               .Include(e => e.AlbumSet)
               .ThenInclude(album => album!.SongSets)
               .Include(e => e.Client)
               .Select(e => _mapper.Map<PurchaseDetail>(e)) // AutoMapper se encarga de la conversión
               .ToListAsync();

        }

        public async Task<PurchaseDetail?> GetByIdAsync(int id)
        {
            //return await _context.PurchaseDetails
            //  .Where(e => e.Id == id)
            //  .Select(e => new PurchaseDetail
            //  {
            //      Id = e.Id,
            //      Client_Id = e.Client_Id,
            //      Album_Id = e.Album_Id,
            //      AlbumSet = e.AlbumSet != null ? new AlbumSet
            //      {
            //          Id = e.AlbumSet.Id,
            //          Name = e.AlbumSet.Name,
            //          Precio = e.AlbumSet.Precio
            //      } : null,
            //      Client = e.Client != null ? new Client
            //      {
            //          Id = e.Client_Id,
            //          Name = e.Client.Name,
            //          Mail = e.Client.Mail,
            //          Direction = e.Client.Direction,
            //          Phone = e.Client.Phone
            //      } : null,
            //      SongSet = e.AlbumSet != null ?
            //        e.AlbumSet.SongSets!.Select(song => new SongSet
            //        {
            //            Id = song.Id
            //        ,
            //            Name = song.Name
            //        ,
            //            Album_Id = song.Album_Id
            //        }).ToList() : new List<SongSet>()
            //  }).FirstOrDefaultAsync();

            return await _context.PurchaseDetails
                .Where(e => e.Id == id)
                .Include(e => e.AlbumSet)
                .ThenInclude(album => album!.SongSets)
                .Include(e => e.Client)
                .ProjectTo<PurchaseDetail>(_mapper.ConfigurationProvider) // Usando AutoMapper
                .FirstOrDefaultAsync();

        }

        public async Task<OperationResult> AddAsync(PurchaseDetail purchaseDetail)
        {
            _context.PurchaseDetails.Add(purchaseDetail);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Compra registrada correctamente.");
        }


        public async Task<OperationResult> DeleteAsync(int id)
        {
            var purchaseDetail = await _context.PurchaseDetails.FindAsync(id);
            if (purchaseDetail == null)
                return OperationResult.Fail("Compra no encontrada.");

            _context.PurchaseDetails.Remove(purchaseDetail);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Compra eliminada correctamente.");

        }

    }
}
