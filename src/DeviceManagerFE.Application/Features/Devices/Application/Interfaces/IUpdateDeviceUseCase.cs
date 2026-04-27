using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IUpdateDeviceUseCase
{
    Task<CreateDeviceResultDto> ExecuteAsync(
        int deviceId,
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default);
}
