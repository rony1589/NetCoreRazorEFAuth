using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicRadio.Core.Entities;


namespace MusicRadio.Infrastructure.Configurations
{
    public class SongSetConfiguration: IEntityTypeConfiguration<SongSet>
    {
        public void Configure(EntityTypeBuilder<SongSet> builder)
        {
            builder.ToTable("SongSet");

            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.Id)
                   .HasColumnType("int")
                   .ValueGeneratedOnAdd()  // Generación automática
                   .UseIdentityColumn();  // Identity(1,1) en SQL Server

            builder.Property(s => s.Name)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired();

            builder.Property(s => s.Album_Id)
                   .IsRequired();

            builder.HasOne(s => s.AlbumSet)
                   .WithMany(a => a.SongSets)
                   .HasForeignKey(s => s.Album_Id)
                   .OnDelete(DeleteBehavior.Cascade);

            // Mapeo de procedimientos almacenados
            // Procedimiento para INSERT
            builder.InsertUsingStoredProcedure("sp_CreateSongSet",
                sp =>
                {
                    sp.HasParameter(s => s.Name);
                    sp.HasParameter(s => s.Album_Id);
                    sp.HasResultColumn(s => s.Id); // Para obtener el ID generado
                });

            // Procedimiento para UPDATE
            builder.UpdateUsingStoredProcedure("sp_UpdateSongSet",
                sp =>
                {
                    sp.HasOriginalValueParameter(s => s.Id);
                    sp.HasParameter(s => s.Name);
                    sp.HasParameter(s => s.Album_Id);
                });

            // Procedimiento para DELETE
            builder.DeleteUsingStoredProcedure("sp_DeleteSongSet",
                sp =>
                {
                    sp.HasOriginalValueParameter(s => s.Id);
                });
        }
    }
}
