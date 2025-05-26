using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicRadio.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicRadio.Infrastructure.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Client");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .HasColumnType("nvarchar(10)")
                   .HasMaxLength(10)
                   .IsRequired();

            builder.Property(c => c.Name)
                   .HasColumnType("nvarchar(100)")
                   .HasMaxLength(100)
                   .IsRequired();
                   

            builder.Property(c => c.Mail)
                   .HasColumnType("nvarchar(50)")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(c => c.Direction)
                  .HasColumnType("nvarchar(500)")
                  .HasMaxLength(500)
                  .IsRequired();

            builder.Property(c => c.Phone)
                  .HasColumnType("nvarchar(20)")
                  .HasMaxLength(20)
                  .IsRequired();

            builder.Property(c => c.Password)
                   .HasColumnType("nvarchar(100)")
                   .HasMaxLength(100) // se define asi ya que al guardar una contraseña segura debe ser comominimo base64 y el tamaño indicado en el requerimiento no cumpliria.
                   .IsRequired();

            builder.Property(c => c.Role_Id)
                   .HasColumnType("int")
                   .IsRequired();

            builder.HasOne(c => c.Role)
                   .WithMany(r => r.Clients)
                   .HasForeignKey(c => c.Role_Id)
                   .OnDelete(DeleteBehavior.Restrict);

            // Insertar datos iniciales
            builder.HasData(
                new Client { Id = "12345", Name = "Admin Inventario",Mail="admininventario@music.com",Direction="Edificio Music Radico", Phone="3105588986", Password= "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=",Role_Id= 2}
            );
        }
    }
}
