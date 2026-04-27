using DeviceEntity = DeviceManagerBE.Domain.Entities.Device;

namespace DeviceManagerBE.Application.Services.Device;

public interface IDeviceWriteRepository
{
    Task<bool> DeviceCodeExistsAsync(string deviceCode, CancellationToken cancellationToken = default);
    Task<bool> DeviceCodeExistsExceptAsync(string deviceCode, int deviceId, CancellationToken cancellationToken = default);
    Task<bool> CategoryExistsAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<bool> EmployeeExistsAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<DeviceEntity?> GetByIdAsync(int deviceId, CancellationToken cancellationToken = default);
    Task<DeviceEntity> AddAsync(DeviceEntity device, CancellationToken cancellationToken = default);
    Task<DeviceEntity> UpdateAsync(DeviceEntity device, CancellationToken cancellationToken = default);
}
