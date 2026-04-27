using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IDeviceInventoryWriteRepository
{
    Task<CreateDeviceResultDto> CreateDeviceAsync(
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default);
}
