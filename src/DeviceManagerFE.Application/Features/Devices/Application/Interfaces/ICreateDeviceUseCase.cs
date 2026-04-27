using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface ICreateDeviceUseCase
{
    Task<CreateDeviceResultDto> ExecuteAsync(
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default);
}
