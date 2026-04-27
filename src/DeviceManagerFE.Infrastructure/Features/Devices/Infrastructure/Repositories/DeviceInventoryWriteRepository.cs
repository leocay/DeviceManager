using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Services;

namespace DeviceManagerFE.Features.Devices.Infrastructure.Repositories;

public sealed class DeviceInventoryWriteRepository : IDeviceInventoryWriteRepository
{
    private readonly DeviceApiClient _deviceApiClient;

    public DeviceInventoryWriteRepository(DeviceApiClient deviceApiClient)
    {
        _deviceApiClient = deviceApiClient;
    }

    public Task<CreateDeviceResultDto> CreateDeviceAsync(
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
        => _deviceApiClient.CreateDeviceAsync(request, cancellationToken);

    public Task<CreateDeviceResultDto> UpdateDeviceAsync(
        int deviceId,
        CreateDeviceRequestDto request,
        CancellationToken cancellationToken = default)
        => _deviceApiClient.UpdateDeviceAsync(deviceId, request, cancellationToken);
}
