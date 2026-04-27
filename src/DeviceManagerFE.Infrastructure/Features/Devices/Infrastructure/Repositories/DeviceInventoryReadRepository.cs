using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Features.Devices.Domain.Entities;
using DeviceManagerFE.Features.Devices.Domain.ValueObjects;
using DeviceManagerFE.Services;

namespace DeviceManagerFE.Features.Devices.Infrastructure.Repositories;

public sealed class DeviceInventoryReadRepository : IDeviceInventoryReadRepository
{
    private readonly DeviceApiClient _deviceApiClient;

    public DeviceInventoryReadRepository(DeviceApiClient deviceApiClient)
    {
        _deviceApiClient = deviceApiClient;
    }

    public async Task<PagedResult<DeviceEntity>?> GetDevicesAsync(
        DeviceInventoryQuery query,
        CancellationToken cancellationToken = default)
    {
        var response = await _deviceApiClient.GetDevicesAsync(
            query.SearchTerm,
            query.CategoryId,
            query.Status,
            query.PageNumber,
            query.PageSize,
            cancellationToken);

        if (response is null)
        {
            return null;
        }

        var items = response.Devices
            .Select(device => new DeviceEntity
            {
                DeviceId = device.DeviceId,
                DeviceName = device.DeviceName,
                CategoryName = device.CategoryName,
                SerialNumber = device.SerialNumber,
                Status = device.Status,
                Brand = device.Brand,
                Model = device.Model
            })
            .ToList();

        return new PagedResult<DeviceEntity>
        {
            Items = items,
            TotalCount = response.TotalCount,
            TotalPages = response.TotalPages
        };
    }

    public Task<DeviceEditorDto?> GetDeviceByIdAsync(
        int deviceId,
        CancellationToken cancellationToken = default)
        => _deviceApiClient.GetDeviceByIdAsync(deviceId, cancellationToken);
}
