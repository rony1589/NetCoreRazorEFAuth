using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicRadio.Core.Interfaces;
using MusicRadio.Infrastructure.Data;
using MusicRadio.Shared.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Infrastructure.Services
{
    public class AuthService(MusicRadioDbContext context, ITokenGenerator tokenGenerator, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly MusicRadioDbContext _context = context;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<OperationResult> LoginAsync(string email, string password)
        {
            var client = await _context.Client.FirstOrDefaultAsync(c => c.Mail == email);
            if (client == null)
                return OperationResult<string>.Fail("El correo electrónico no está registrado.");

            if (!PasswordHasher.VerifyPassword(password, client.Password))
                return OperationResult<string>.Fail("Contraseña incorrecta.");
            
            //1. Generamos el token 
            var token = _tokenGenerator.GenerateToken(client);

            // 2. Decodificar el token para extraer las claims
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToList();

            // 3.Crear identidad y firmar(PARA RAZOR PAGES)
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                AllowRefresh = true
            };


            await _httpContextAccessor.HttpContext!.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                   authProperties);

            // 4. Opcional: Guardar el token en cookie para APIs
            _httpContextAccessor.HttpContext!.Response.Cookies.Append(
                "MusicRadio.JWT",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

            return OperationResult.Ok(token);
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;
           
            // 1. Cerrar sesión en el sistema de autenticación (¡ESENCIAL!)
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 2. Eliminar cookies manualmente (opcional pero recomendado)
            httpContext.Response.Cookies.Delete("MusicRadio.JWT");
            httpContext.Response.Cookies.Delete("MusicRadio.Auth"); // Si usas otra cookie
            
        }

    }
}
