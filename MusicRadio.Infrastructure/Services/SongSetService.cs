using Microsoft.EntityFrameworkCore;
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
    public class SongSetService(MusicRadioDbContext _context) : ISongSetService
    {
        public async Task<IEnumerable<SongSet>> GetAllAsync()
        {
            return await _context.SongSet
                .Include(e => e.AlbumSet)
                .Select(e => new SongSet
                {
                    Id = e.Id,
                    Name = e.Name,
                    Album_Id = e.Album_Id,
                    AlbumSet = e.AlbumSet != null ? new AlbumSet
                    {
                        Id = e.AlbumSet.Id,
                        Name = e.AlbumSet.Name,
                        Precio = e.AlbumSet.Precio
                    } : null
                }).ToListAsync();
        }

        public async Task<SongSet?> GetByIdAsync(int id)
        {
            return await _context.SongSet
                 .Where(e => e.Id == id)
                 .Select(e => new SongSet
                 {
                     Id = e.Id,
                     Name = e.Name,
                     Album_Id = e.Album_Id,
                     AlbumSet = e.AlbumSet != null ? new AlbumSet
                     {
                         Id = e.AlbumSet.Id,
                         Name = e.AlbumSet.Name,
                         Precio = e.AlbumSet.Precio
                     } : null
                 }).FirstOrDefaultAsync();
        }

        public async Task<OperationResult> AddAsync(SongSet songSet)
        {
            try
            {
                var result = (await _context.Database
                   .SqlQueryRaw<SpResultWithId>("EXEC sp_CreateSongSet @Name = {0}, @Album_Id = {1}", songSet.Name, songSet.Album_Id)
                   .ToListAsync())
                   .FirstOrDefault();

                if (result == null)
                    return OperationResult.Fail("Error desconocido al crear la canción");

                if (result.Id <= 0)
                {
                    return result.Id switch
                    {
                        SpErrorCodes.RelatedRecordNotFound =>
                            OperationResult.Fail("Álbum no encontrado"),
                        SpErrorCodes.DuplicateRecord =>
                            OperationResult.Fail("Ya existe una canción con ese nombre en el álbum"),
                        SpErrorCodes.UnexpectedError =>
                            OperationResult.Fail($"Error en BD: {result.ErrorMessage}"),
                        _ => OperationResult.Fail("No se pudo crear la canción")
                    };
                }

                songSet.Id =Convert.ToInt32(result.Id);
                return OperationResult.Ok($"Canción '{songSet.Name}' creada correctamente!");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Error inesperado: {ex.Message}");
            }
        }

        public async Task<OperationResult> UpdateAsync(SongSet songSet)
        {
            try
            {
                var result = (await _context.Database
                   .SqlQueryRaw<SpResultWithId>("EXEC sp_UpdateSongSet @Id = {0}, @Name = {1}, @Album_Id = {2}",
                               songSet.Id, songSet.Name, songSet.Album_Id)
                   .ToListAsync())
                   .FirstOrDefault();


                if (result == null)
                    return OperationResult.Fail("Error desconocido al actualizar la canción");

                if (result.Id <= 0)
                {
                    return result.Id switch
                    {
                        SpErrorCodes.RecordNotFound =>
                            OperationResult.Fail("Canción no encontrada"),
                        SpErrorCodes.RelatedRecordNotFound =>
                            OperationResult.Fail("Álbum no encontrado"),
                        SpErrorCodes.DuplicateRecord =>
                            OperationResult.Fail("Ya existe otra canción con ese nombre en el álbum"),
                        SpErrorCodes.UnexpectedError =>
                            OperationResult.Fail($"Error en BD: {result.ErrorMessage}"),
                        _ => OperationResult.Fail("No se pudo actualizar la canción")
                    };
                }

                return OperationResult.Ok($"Canción '{songSet.Name}' actualizada correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Error inesperado: {ex.Message}");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var result = (await _context.Database
                   .SqlQueryRaw<SpResultWithId>("EXEC sp_DeleteSongSet @Id = {0}", id)
                   .ToListAsync())
                   .FirstOrDefault();

                if (result == null)
                    return OperationResult.Fail("Error desconocido al eliminar la canción");

                if (result.Id <= 0)
                {
                    return result.Id switch
                    {
                        SpErrorCodes.RecordNotFound =>
                            OperationResult.Fail("Canción no encontrada"),
                        SpErrorCodes.UnexpectedError =>
                            OperationResult.Fail($"Error en BD: {result.ErrorMessage}"),
                        _ => OperationResult.Fail("No se pudo eliminar la canción")
                    };
                }

                return OperationResult.Ok("Canción eliminada correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Error inesperado: {ex.Message}");
            }
        }

    }
}
