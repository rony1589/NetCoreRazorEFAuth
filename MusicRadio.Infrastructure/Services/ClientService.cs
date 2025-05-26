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
    public class ClientService(MusicRadioDbContext _context) : IClientService
    {
        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await _context.Client.ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(string id)
        {
            return await _context.Client.FindAsync(id);
        }

        public async Task<OperationResult> AddAsync(Client client)
        {
            bool exists = await _context.Client.AnyAsync(s => s.Mail == client.Mail);
            if (exists)
                return OperationResult.Fail($"El usuario ya existe con el Email: {client.Mail}");

            bool existsI = await _context.Client.AnyAsync(s => s.Mail == client.Mail || s.Id == client.Id);
            if (existsI)
                return OperationResult.Fail($"El usuario ya existe con la Identifiación: {client.Id}");

            _context.Client.Add(client);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Usuario registrado correctamente.");
        }

        public async Task<OperationResult> UpdateAsync(Client client)
        {
            bool exists = await _context.Client
            .AnyAsync(s => s.Id != client.Id && s.Mail == client.Mail);

            if (exists)
                return OperationResult.Fail($"Otro cliente ya tiene ese Email: {client.Mail}");

            _context.Client.Update(client);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Usuario actualizado correctamente.");

        }

        public async Task<OperationResult> DeleteAsync(string id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client == null)
                return OperationResult.Fail("Usuario no encontrado.");

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Usuario eliminado correctamente.");

        }

    }
}
