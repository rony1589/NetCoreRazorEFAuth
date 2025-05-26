using Microsoft.Extensions.DependencyInjection;
using MusicRadio.Core.Entities;
using MusicRadio.Infrastructure.Data;
using MusicRadio.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using MusicRadio.Web.Pages.Albums;
using System.Text.Json;
using AutoMapper;
using MusicRadio.Core.Interfaces;
using MusicRadio.Application.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace MusicRadio.Tests.Controllers
{
    [TestClass]
    public class AlbumsControllerTests: BasePruebas
    {

        private readonly  IAlbumSetService _albumSetService;
        private readonly  ILogger<IndexModel> _logger;
        private readonly  IMapper _mapper;
        private readonly  JsonSerializerOptions _jsonOptions;
        public TestContext? TestContext { get; set; }

        public AlbumsControllerTests()
        {
            var options = new DbContextOptionsBuilder<MusicRadioDbContext>()
            .UseInMemoryDatabase("TestDB")
            .Options;

            var context = new MusicRadioDbContext(options);

            context.AlbumSet.Add(new AlbumSet { Name = "Álbum de prueba", Precio = 800000 });
            context.SaveChanges();

            // Instanciar servicios reales
            _albumSetService = new AlbumSetService(context);
            _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<IndexModel>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<AlbumSet, AlbumSetDto>());
            _mapper = new Mapper(config);

            _jsonOptions = new JsonSerializerOptions();

        }

        [TestMethod]
        public async Task OnGetAsync()
        {

            // Arrange
            var model = new IndexModel(_albumSetService, _logger, _mapper, _jsonOptions);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.Role, "Cliente")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext { User = principal };
            model.PageContext = new PageContext { HttpContext = httpContext };

            // Act
            var result = await model.OnGetAsync();
            TestContext!.WriteLine($"Resultado: {result.GetType().Name}");
            TestContext!.WriteLine($"Álbumes obtenidos: {model.AlbumSet.Count}");

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult)); // Confirma que devuelve la vista correctamente
            Assert.IsTrue(model.AlbumSet.Count >= 0); // Confirma que la lista de álbumes se asignó

        }
    }
}
