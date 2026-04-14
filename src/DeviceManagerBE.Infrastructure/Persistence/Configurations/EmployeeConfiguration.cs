using DeviceManagerBE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManagerBE.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.EmployeeId);

        builder.Property(x => x.EmployeeCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.EmployeeCode)
            .IsUnique();

        builder.Property(x => x.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Department)
            .HasMaxLength(100);

        builder.Property(x => x.Position)
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();
    }
}

