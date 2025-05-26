using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public class AlbumSetService(MusicRadioDbContext _context) : IAlbumSetService
    {
        public async Task<IEnumerable<AlbumSet>> GetAllAsync()
        {
            return await _context.AlbumSet.ToListAsync();
        }

        public async Task<AlbumSet?> GetByIdAsync(int id)
        {
            return await _context.AlbumSet.FindAsync(id);
        }

        public async Task<OperationResult> AddAsync(AlbumSet albumSet)
        {
            try
            {
                var result = (await _context.Database
                 .SqlQueryRaw<SpResultWithId>("EXEC sp_CreateAlbumSet @Name = {0}, @Precio = {1}", albumSet.Name, albumSet.Precio)
                 .ToListAsync())
                 .FirstOrDefault();


                if (result == null)
                    return OperationResult.Fail("Error desconocido al crear el álbum");

                if (result.Id <= 0)
                {
                    return result.Id switch
                    {
                        SpErrorCodes.DuplicateRecord =>
                            OperationResult.Fail($"Ya existe un álbum con el nombre: {albumSet.Name}"),
                        SpErrorCodes.UnexpectedError =>
                            OperationResult.Fail($"Error en BD: {result.ErrorMessage}"),
                        _ => OperationResult.Fail("No se pudo crear el álbum")
                    };
                }

                albumSet.Id = Convert.ToInt32(result.Id);
                return OperationResult.Ok($"Álbum '{albumSet.Name}' creado correctamente!");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Error inesperado: {ex.Message}");
            }
        }

        public async Task<OperationResult> UpdateAsync(AlbumSet albumSet)
        {
            try
            {
                var result = (await _context.Database
                .SqlQueryRaw<SpResultWithId>("EXEC sp_UpdateAlbumSet @Id = {0}, @Name = {1}, @Precio = {2}",
                               albumSet.Id, albumSet.Name, albumSet.Precio)
                .ToListAsync())
                .FirstOrDefault();


                if (result == null)
                    return OperationResult.Fail("Error desconocido al actualizar el álbum");

                if (result.Id <= 0)
                {
                    return result.Id switch
                    {
                        SpErrorCodes.RecordNotFound =>
                            OperationResult.Fail("Álbum no encontrado"),
                        SpErrorCodes.DuplicateRecord =>
                            OperationResult.Fail("Ya existe otro álbum con ese nombre"),
                        SpErrorCodes.UnexpectedError =>
                            OperationResult.Fail($"Error en BD: {result.ErrorMessage}"),
                        _ => OperationResult.Fail("No se pudo actualizar el álbum")
                    };
                }

                return OperationResult.Ok($"Álbum '{albumSet.Name}' actualizado correctamente");
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
                    .SqlQueryRaw<SpResultWithId>("EXEC sp_DeleteAlbumSet @Id = {0}", id)
                    .ToListAsync())
                    .FirstOrDefault();

                if (result == null)
                    return OperationResult.Fail("Error desconocido al eliminar el álbum");

                if (result.Id <= 0)
                {
                    return result.Id switch
                    {
                        SpErrorCodes.RecordNotFound =>
                            OperationResult.Fail("Álbum no encontrado"),
                        SpErrorCodes.DependentRecord =>
                            OperationResult.Fail("No se puede eliminar, tiene canciones asociadas"),
                        SpErrorCodes.UnexpectedError =>
                            OperationResult.Fail($"Error en BD: {result.ErrorMessage}"),
                        _ => OperationResult.Fail("No se pudo eliminar el álbum")
                    };
                }

                return OperationResult.Ok("Álbum eliminado correctamente");
            }
            catch (Exception ex)
            {
                return OperationResult.Fail($"Error inesperado: {ex.Message}");
            }

        }

    }
}
