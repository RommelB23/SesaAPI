using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SesaAPI.Data.Models;

namespace SesaAPI.Data.Configurations
{
    public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.ToTable("Policies");

            builder.HasKey(x => x.Id)
                   .HasName("PK_Policies_Id");

            builder.Property(x => x.PolicyNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(x => x.PolicyNumber)
                   .IsUnique()
                   .HasDatabaseName("UQ_Policies_PolicyNumber");

            builder.Property(x => x.IssueDate)
                   .IsRequired();

            builder.Property(x => x.InsuredAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.TotalPremium)
                   .HasColumnType("decimal(18,2)");

            builder.Property(x => x.IsActive)
                   .HasDefaultValueSql("0");

            builder.Property(x => x.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Customer)
                   .WithMany(c => c.Policies)
                   .HasForeignKey(x => x.CustomerId)
                   .HasConstraintName("FK_Policies_Customers");

            builder.HasOne(x => x.Vehicle)
                   .WithMany(v => v.Policies)
                   .HasForeignKey(x => x.VehicleId)
                   .HasConstraintName("FK_Policies_Vehicles");
        }
    }
}
