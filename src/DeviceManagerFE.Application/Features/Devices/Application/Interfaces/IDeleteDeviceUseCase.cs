using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IDeleteDeviceUseCase
{
    Task<CreateDeviceResultDto> ExecuteAsync(
        int deviceId,
        CancellationToken cancellationToken = default);
}