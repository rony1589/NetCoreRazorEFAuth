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
    public class AlbumSetConfiguration : IEntityTypeConfiguration<AlbumSet>
    {
        public void Configure(EntityTypeBuilder<AlbumSet> builder)
        {
            builder.ToTable("AlbumSet");

            builder.HasKey(s => s.Id);

            builder.Property(p => p.Id)
                   .HasColumnType("int")
                   .ValueGeneratedOnAdd()  // Generación automática
                   .UseIdentityColumn();  // Identity(1,1) en SQL Server

            builder.Property(s => s.Name)
                   .HasColumnType("nvarchar(100)")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Precio)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();


            builder.HasMany(s => s.SongSets)
                   .WithOne(a => a.AlbumSet)
                   .HasForeignKey(s => s.Album_Id)
                   .OnDelete(DeleteBehavior.Cascade);

            // Mapeo de procedimientos almacenados
            // Procedimiento para INSERT
            builder.InsertUsingStoredProcedure("sp_CreateAlbumSet",
                sp =>
                {
                    sp.HasParameter(s => s.Name);
                    sp.HasParameter(s => s.Precio);
                    sp.HasResultColumn(s => s.Id); // Para obtener el ID generado
                });

            // Procedimiento para UPDATE
            builder.UpdateUsingStoredProcedure("sp_UpdateAlbumSet",
                sp =>
                {
                    sp.HasOriginalValueParameter(s => s.Id);
                    sp.HasParameter(s => s.Name);
                    sp.HasParameter(s => s.Precio);
                });

            // Procedimiento para DELETE
            builder.DeleteUsingStoredProcedure("sp_DeleteAlbumSet",
                sp =>
                {
                    sp.HasOriginalValueParameter(s => s.Id);
                });
        }
    }
}
