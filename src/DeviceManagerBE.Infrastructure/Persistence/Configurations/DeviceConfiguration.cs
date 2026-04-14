using DeviceManagerBE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManagerBE.Infrastructure.Persistence.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices", tableBuilder =>
        {
            tableBuilder.HasCheckConstraint("CK_Devices_Quantity", "[Quantity] > 0");
        });

        builder.HasKey(x => x.DeviceId);

        builder.Property(x => x.DeviceCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.DeviceCode)
            .IsUnique();

        builder.Property(x => x.DeviceName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Brand)
            .HasMaxLength(100);

        builder.Property(x => x.Model)
            .HasMaxLength(100);

        builder.Property(x => x.SerialNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasDefaultValue(1)
            .IsRequired();

        builder.Property(x => x.Location)
            .HasMaxLength(100);

        builder.Property(x => x.Note)
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.HasIndex(x => x.CategoryId);
        builder.HasIndex(x => x.EmployeeId);
        builder.HasIndex(x => x.Status);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Devices)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.Devices)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

