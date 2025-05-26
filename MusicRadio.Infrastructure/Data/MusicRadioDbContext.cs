using Microsoft.EntityFrameworkCore;
using MusicRadio.Core.Entities;
using MusicRadio.Shared.Common;
using System.Reflection;

namespace MusicRadio.Infrastructure.Data
{
    public class MusicRadioDbContext(DbContextOptions<MusicRadioDbContext> options):DbContext(options)
    {
        public DbSet<Client> Client => Set<Client>();
        public DbSet<Role> Role => Set<Role>();
        public DbSet<AlbumSet> AlbumSet => Set<AlbumSet>();
        public DbSet<SongSet> SongSet => Set<SongSet>();
        public DbSet<PurchaseDetail> PurchaseDetails => Set<PurchaseDetail>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Aplicar todas las configuraciones automáticamente
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            //var model = modelBuilder.Model.GetEntityTypes()
            //    .Where(e => e.Name.Contains("SongSet"))
            //    .SelectMany(e => e.GetProperties())
            //    .Select(p => p.Name)
            //    .ToList();

            //    Console.WriteLine(string.Join(", ", model));

            //var entityType = modelBuilder.Model.FindEntityType(typeof(SongSet));

            //foreach (var property in entityType.GetProperties())
            //{
            //    Console.WriteLine($"Propiedad: {property.Name} | Definida en: {property.DeclaringEntityType.Name}");
            //}

        }

    }
}

