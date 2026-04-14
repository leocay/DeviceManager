using DeviceManagerBE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeviceManagerBE.Infrastructure.Persistence.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");

        builder.HasKey(x => x.AdminId);

        builder.Property(x => x.Username)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Username)
            .IsUnique();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()")
            .IsRequired();

        builder.HasData(new Admin
        {
            AdminId = AdminSeed.DefaultAdminId,
            Username = AdminSeed.DefaultUsername,
            PasswordHash = AdminSeed.DefaultPasswordHash,
            FullName = AdminSeed.DefaultFullName,
            Email = null,
            CreatedAt = AdminSeed.DefaultCreatedAt
        });
    }
}

