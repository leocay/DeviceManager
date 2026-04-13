using DeviceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceManager.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins => Set<Admin>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<DeviceCategory> DeviceCategories => Set<DeviceCategory>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<DeviceLog> DeviceLogs => Set<DeviceLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
