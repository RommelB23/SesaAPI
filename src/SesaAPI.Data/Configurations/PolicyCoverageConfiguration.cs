using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SesaAPI.Data.Models;

namespace SesaAPI.Data.Configurations
{
    public class PolicyCoverageConfiguration : IEntityTypeConfiguration<PolicyCoverage>
    {
        public void Configure(EntityTypeBuilder<PolicyCoverage> builder)
        {
            builder.ToTable("PolicyCoverages");

            builder.HasKey(x => new { x.Id })
                   .HasName("PK_PolicyCoverages_Id");

            builder.HasOne(x => x.Policy)
                   .WithMany(p => p.PolicyCoverages)
                   .HasForeignKey(x => x.PolicyId)
                   .HasConstraintName("FK_PolicyCoverages_Policies");

            builder.HasOne(x => x.Coverage)
                   .WithMany(c => c.PolicyCoverages)
                   .HasForeignKey(x => x.CoverageId)
                   .HasConstraintName("FK_PolicyCoverages_Coverages");
        }
    }
}
