using CBP.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CBP.Data.Configurations
{
    public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", schema: "biz");

            builder.Property(s => s.FirstName)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(s => s.LastName)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(s => s.Email)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
