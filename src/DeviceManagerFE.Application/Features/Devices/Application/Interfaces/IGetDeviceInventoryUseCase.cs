using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IGetDeviceInventoryUseCase
{
    Task<GetDeviceInventoryResultDto?> ExecuteAsync(
        GetDeviceInventoryRequestDto request,
        CancellationToken cancellationToken = default);
}

