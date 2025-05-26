using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicRadio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Infrastructure.Configurations
{
    public class RoleConfiguration: IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                   .HasColumnType("int")
                   .ValueGeneratedOnAdd()  // Generación automática
                   .UseIdentityColumn();  // Identity(1,1) en SQL Server

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasMany(s => s.Clients)
                  .WithOne(a => a.Role)
                  .HasForeignKey(s => s.Role_Id)
                  .OnDelete(DeleteBehavior.Cascade);

            // Insertar datos iniciales
            builder.HasData(
                new Role { Id = 1, Name = "Cliente" },
                new Role { Id = 2, Name = "Director Inventario" }
            );

        }
    }
}
