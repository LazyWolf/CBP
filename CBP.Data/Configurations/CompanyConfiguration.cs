using CBP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CBP.Data.Configurations
{
    public sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies", schema: "biz");

            builder.Property(s => s.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
