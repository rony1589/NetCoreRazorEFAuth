using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Infrastructure.Services
{
    public class TokenGeneratorService(IConfiguration configuration, MusicRadioDbContext context) : ITokenGenerator
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly MusicRadioDbContext _context = context;

        public string GenerateToken(Client client)
        {
            var roleName = _context.Role
            .Where(r => r.Id == client.Role_Id)
            .Select(r => r.Name)
            .FirstOrDefault();

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, client.Id.ToString()),
            new Claim(ClaimTypes.Email, client.Mail),
            new Claim(ClaimTypes.Name, client.Name),
            new Claim(ClaimTypes.Role, roleName!.ToString().ToUpper())
            // Agrega más claims según sea necesario
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
