using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;

namespace DeviceManagerFE.Features.Devices.Application.UseCases;

public sealed class DeleteDeviceUseCase : IDeleteDeviceUseCase
{
    private readonly IDeviceInventoryWriteRepository _deviceInventoryWriteRepository;

    public DeleteDeviceUseCase(IDeviceInventoryWriteRepository deviceInventoryWriteRepository)
    {
        _deviceInventoryWriteRepository = deviceInventoryWriteRepository;
    }

    public Task<CreateDeviceResultDto> ExecuteAsync(
        int deviceId,
        CancellationToken cancellationToken = default)
        => _deviceInventoryWriteRepository.DeleteDeviceAsync(deviceId, cancellationToken);
}