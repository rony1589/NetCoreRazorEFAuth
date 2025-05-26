using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicRadio.Infrastructure.Data;
using MusicRadio.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Tests
{
    public class BasePruebas
    {
        protected IServiceProvider CreateContext(string nameDB)
        {
            var services = new ServiceCollection();
            services.AddDbContext<MusicRadioDbContext>(options => options.UseInMemoryDatabase(databaseName: nameDB),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped);

            return services.BuildServiceProvider();
        }

        protected IMapper ConfigurarAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return config.CreateMapper();
        }

    }
}
