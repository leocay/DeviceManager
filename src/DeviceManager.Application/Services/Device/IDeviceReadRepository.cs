using DeviceManager.Application.DTOs.Device;

namespace DeviceManager.Application.Services.Device;

public interface IDeviceReadRepository
{
    Task<GetDevicesResultDto> GetDevicesPaginatedAsync(
        string? searchTerm,
        string? categoryId,
        string? status,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);
}
