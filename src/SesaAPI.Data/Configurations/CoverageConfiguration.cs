using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SesaAPI.Data.Models;

namespace SesaAPI.Data.Configurations
{
    public class CoverageConfiguration : IEntityTypeConfiguration<Coverage>
    {
        public void Configure(EntityTypeBuilder<Coverage> builder)
        {
            builder.ToTable("Coverages");

            builder.HasKey(x => x.Id)
                   .HasName("PK_Coverages_Id");

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Rate)
                   .HasColumnType("decimal(5,2)");

            builder.Property(x => x.IsActive)
                   .HasDefaultValueSql("1");

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
