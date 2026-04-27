using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;

namespace DeviceManagerFE.Features.Devices.Application.UseCases;

public sealed class CreateDeviceUseCase : ICreateDeviceUseCase
{
    private readonly IDeviceInventoryWriteRepository _deviceInventoryWriteRepository;

    public CreateDeviceUseCase(IDeviceInventoryWriteRepository deviceInventoryWriteRepository)
    {
        _deviceInventoryWriteRepository = deviceInventoryWriteRepository;
    }

    public Task<CreateDeviceResultDto> ExecuteAsync(
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
        => _deviceInventoryWriteRepository.CreateDeviceAsync(request, cancellationToken);
}
