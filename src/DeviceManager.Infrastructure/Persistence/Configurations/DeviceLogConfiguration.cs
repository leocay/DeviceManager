using DeviceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManager.Infrastructure.Persistence.Configurations;

public class DeviceLogConfiguration : IEntityTypeConfiguration<DeviceLog>
{
    public void Configure(EntityTypeBuilder<DeviceLog> builder)
    {
        builder.ToTable("DeviceLogs");

        builder.HasKey(x => x.LogId);

        builder.Property(x => x.ActionType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ActionBy)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ActionTime)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.Property(x => x.Content)
            .HasMaxLength(500);

        builder.HasIndex(x => x.DeviceId);
        builder.HasIndex(x => x.ActionTime);

        builder.HasOne(x => x.Device)
            .WithMany(x => x.DeviceLogs)
            .HasForeignKey(x => x.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
