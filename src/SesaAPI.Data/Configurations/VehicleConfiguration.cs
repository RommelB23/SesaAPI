using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SesaAPI.Data.Models;

namespace SesaAPI.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(x => x.Id)
                   .HasName("PK_Vehicles_Id");

            builder.Property(x => x.LicensePlate)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(x => x.Brand)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Model)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Year)
                   .IsRequired();

            builder.Property(x => x.CommercialValue)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IsActive)
                   .HasDefaultValueSql("1");

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
