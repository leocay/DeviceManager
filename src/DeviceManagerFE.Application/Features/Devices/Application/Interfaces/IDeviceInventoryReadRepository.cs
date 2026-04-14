using DeviceManagerFE.Features.Devices.Domain.Entities;
using DeviceManagerFE.Features.Devices.Domain.ValueObjects;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IDeviceInventoryReadRepository
{
    Task<PagedResult<DeviceEntity>?> GetDevicesAsync(
        DeviceInventoryQuery query,
        CancellationToken cancellationToken = default);
}

