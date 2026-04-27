using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IGetDeviceDetailUseCase
{
    Task<DeviceEditorDto?> ExecuteAsync(int deviceId, CancellationToken cancellationToken = default);
}
