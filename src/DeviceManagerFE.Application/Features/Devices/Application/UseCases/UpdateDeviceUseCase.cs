using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;

namespace DeviceManagerFE.Features.Devices.Application.UseCases;

public sealed class UpdateDeviceUseCase : IUpdateDeviceUseCase
{
    private readonly IDeviceInventoryWriteRepository _deviceInventoryWriteRepository;

    public UpdateDeviceUseCase(IDeviceInventoryWriteRepository deviceInventoryWriteRepository)
    {
        _deviceInventoryWriteRepository = deviceInventoryWriteRepository;
    }

    public Task<CreateDeviceResultDto> ExecuteAsync(
        int deviceId,
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
        => _deviceInventoryWriteRepository.UpdateDeviceAsync(deviceId, request, cancellationToken);
}
