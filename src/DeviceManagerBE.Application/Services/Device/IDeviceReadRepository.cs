using DeviceManagerBE.Application.DTOs.Device;

namespace DeviceManagerBE.Application.Services.Device;

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

