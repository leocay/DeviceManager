using DeviceManagerBE.Application.Services.Device;
using DeviceManagerBE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DeviceEntity = DeviceManagerBE.Domain.Entities.Device;

namespace DeviceManagerBE.Infrastructure.Repositories.Device;

public sealed class DeviceWriteRepository : IDeviceWriteRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DeviceWriteRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> DeviceCodeExistsAsync(string deviceCode, CancellationToken cancellationToken = default)
        => _dbContext.Devices.AnyAsync(device => device.DeviceCode == deviceCode, cancellationToken);

    public Task<bool> DeviceCodeExistsExceptAsync(string deviceCode, int deviceId, CancellationToken cancellationToken = default)
        => _dbContext.Devices.AnyAsync(
            device => device.DeviceCode == deviceCode && device.DeviceId != deviceId,
            cancellationToken);

    public Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken = default)
        => _dbContext.DeviceCategories.AnyAsync(category => category.CategoryId == categoryId, cancellationToken);

    public Task<bool> EmployeeExistsAsync(int employeeId, CancellationToken cancellationToken = default)
        => _dbContext.Employees.AnyAsync(employee => employee.EmployeeId == employeeId, cancellationToken);

    public Task<DeviceEntity?> GetByIdAsync(int deviceId, CancellationToken cancellationToken = default)
        => _dbContext.Devices.FirstOrDefaultAsync(device => device.DeviceId == deviceId, cancellationToken);

    public async Task<DeviceEntity> AddAsync(DeviceEntity device, CancellationToken cancellationToken = default)
    {
        _dbContext.Devices.Add(device);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return device;
    }

    public async Task<DeviceEntity> UpdateAsync(DeviceEntity device, CancellationToken cancellationToken = default)
    {
        _dbContext.Devices.Update(device);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return device;
    }
}
