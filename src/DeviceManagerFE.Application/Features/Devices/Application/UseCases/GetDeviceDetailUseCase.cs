using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;

namespace DeviceManagerFE.Features.Devices.Application.UseCases;

public sealed class GetDeviceDetailUseCase : IGetDeviceDetailUseCase
{
    private readonly IDeviceInventoryReadRepository _deviceInventoryReadRepository;

    public GetDeviceDetailUseCase(IDeviceInventoryReadRepository deviceInventoryReadRepository)
    {
        _deviceInventoryReadRepository = deviceInventoryReadRepository;
    }

    public Task<DeviceEditorDto?> ExecuteAsync(int deviceId, CancellationToken cancellationToken = default)
        => _deviceInventoryReadRepository.GetDeviceByIdAsync(deviceId, cancellationToken);
}
