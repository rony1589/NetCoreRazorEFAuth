using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MusicRadio.Core.Entities;


namespace MusicRadio.Infrastructure.Configurations
{
    public class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
    {
        public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
        {
            builder.ToTable("PurchaseDetail");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .HasColumnType("int")  
                   .ValueGeneratedOnAdd()  // Generación automática
                   .UseIdentityColumn();  // Identity(1,1) en SQL Server

            builder.Property(p => p.Client_Id)
                    .HasColumnType("nvarchar(10)")
                    .HasMaxLength(10)
                    .IsRequired();

            builder.Property(p => p.Album_Id)
                   .HasColumnType("int")
                   .IsRequired();


            builder.Property(p => p.Total)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.HasOne(p => p.Client)
                   .WithMany(c => c.PurchaseDetails)
                   .HasForeignKey(p => p.Client_Id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.AlbumSet)
                   .WithMany(s => s.PurchaseDetails)
                   .HasForeignKey(p => p.Album_Id)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
