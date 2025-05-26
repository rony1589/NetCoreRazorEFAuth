using Microsoft.Extensions.DependencyInjection;
using MusicRadio.Core.Entities;
using MusicRadio.Infrastructure.Data;
using MusicRadio.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Protocol;
using MusicRadio.Shared.Common;

namespace MusicRadio.Tests.Services
{
    [TestClass]
    public class ClientServiceTests: BasePruebas
    {
        public TestContext? TestContext { get; set; }

        [TestMethod]
        public async Task GetAllAsync()
        {
            //Arrange
            var nameDB = Guid.NewGuid().ToString();

            //Crear contexto y agregar datos
            var serviceProvider1 = CreateContext(nameDB);
            var context1 = serviceProvider1.GetRequiredService<MusicRadioDbContext>();

            context1.Client.Add(new Client() { Id = "985647", Name = "Juan Pérez", Mail = "juan@hotmail.com", Direction = "Medellin", Phone = "3215478", Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", Role_Id = Convert.ToInt32(UserRole.Cliente) });
            context1.Client.Add(new Client() { Id = "9856478", Name = "Juan Carmona", Mail = "juanc@hotmail.com", Direction = "Bogota", Phone = "321547887", Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", Role_Id = Convert.ToInt32(UserRole.Cliente) });
            await context1.SaveChangesAsync();

            //Creamos nuevo contexo para el servicio
            var serviceProvider2 = CreateContext(nameDB);
            var contexto2 = serviceProvider2.GetRequiredService<MusicRadioDbContext>();

            var service = new ClientService(contexto2);

            // Act
            var result = await service.GetAllAsync();
            TestContext!.WriteLine($"Mensaje devuelto: {result.ToJson()}");

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(s => s.Mail == "juan@hotmail.com"));
            Assert.IsTrue(result.Any(s => s.Mail == "juanc@hotmail.com"));

        }

        [TestMethod]
        public async Task CreateAsync()
        {
            // Arrange
            var nameDB = Guid.NewGuid().ToString();
            var serviceProvider = CreateContext(nameDB);
            var context = serviceProvider.GetRequiredService<MusicRadioDbContext>();
            var service = new ClientService(context);

            var client = new Client
            {
                Id = "9856478",
                Name = "Juan Carmona",
                Mail = "juanc@hotmail.com",
                Direction = "Bogota",
                Phone = "321547887",
                Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=",
                Role_Id = Convert.ToInt32(UserRole.Cliente)
            };

            // Act
            var result = await service.AddAsync(client);
            TestContext!.WriteLine($"Mensaje devuelto: {result.Message}");

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Usuario registrado correctamente.", result.Message);

            var saved = await context.Client.FindAsync(client.Id);
            Assert.IsNotNull(saved);
            Assert.AreEqual("Juan Carmona", saved.Name);

        }

        [TestMethod]
        public async Task GetByIdAsyncExist()
        {
            //Arrange
            var nameDB = Guid.NewGuid().ToString();

            //Crear contexto y agregar datos
            var serviceProvider1 = CreateContext(nameDB);
            var context1 = serviceProvider1.GetRequiredService<MusicRadioDbContext>();

            var ClienteId = "985647";

            context1.Client.Add(new Client() { Id = ClienteId, Name = "Juan Pérez", Mail = "juan@hotmail.com", Direction = "Medellin", Phone = "3215478", Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", Role_Id = Convert.ToInt32(UserRole.Cliente) });
            context1.Client.Add(new Client() { Id = "9856478", Name = "Juan Carmona", Mail = "juanc@hotmail.com", Direction = "Bogota", Phone = "321547887", Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", Role_Id = Convert.ToInt32(UserRole.Cliente) });
            await context1.SaveChangesAsync();

            //Creamos nuevo contexo para el servicio
            var serviceProvider2 = CreateContext(nameDB);
            var contexto2 = serviceProvider2.GetRequiredService<MusicRadioDbContext>();

            var service = new ClientService(contexto2);

            // Act
            var result = await service.GetByIdAsync(ClienteId);
            TestContext!.WriteLine($"Mensaje devuelto: {result.ToJson()}");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Juan Pérez", result.Name);
            Assert.AreEqual(ClienteId, result.Id);
        }

    }
}
