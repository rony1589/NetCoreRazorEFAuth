using Microsoft.Extensions.DependencyInjection;
using MusicRadio.Core.Interfaces;
using MusicRadio.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenGenerator, TokenGeneratorService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAlbumSetService, AlbumSetService>();
            services.AddScoped<ISongSetService, SongSetService>();
            services.AddScoped<IPurchaseDetailService, PurchaseDetailService>();

            return services;
        }
    }
}
