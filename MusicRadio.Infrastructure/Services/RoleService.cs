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
    public class RoleService(MusicRadioDbContext _context) : IRoleService
    {
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Role.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Role.FindAsync(id);
        }

        public async Task<OperationResult> AddAsync(Role role)
        {
            bool exists = await _context.Role.AnyAsync(s => s.Name == role.Name);
            if (exists)
                return OperationResult.Fail($"El Rol {role.Name} ya existe.");

            _context.Role.Add(role);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Rol registrado correctamente.");
        }

        public async Task<OperationResult> UpdateAsync(Role role)
        {
            bool exists = await _context.Role
            .AnyAsync(s => s.Id != role.Id && s.Name == role.Name);

            if (exists)
                return OperationResult.Fail($"Otro Role ya tiene el nombre: {role.Name}");

            _context.Role.Update(role);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Rol actualizado correctamente.");

        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
                return OperationResult.Fail("Rol no encontrado.");

            _context.Role.Remove(role);
            await _context.SaveChangesAsync();
            return OperationResult.Ok("Rol eliminado correctamente.");

        }

    }
}
