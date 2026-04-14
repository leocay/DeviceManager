using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Features.Devices.Domain.Rules;
using DeviceManagerFE.Features.Devices.Domain.ValueObjects;

namespace DeviceManagerFE.Features.Devices.Application.UseCases;

public sealed class GetDeviceInventoryUseCase : IGetDeviceInventoryUseCase
{
    private readonly IDeviceInventoryReadRepository _repository;

    public GetDeviceInventoryUseCase(IDeviceInventoryReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetDeviceInventoryResultDto?> ExecuteAsync(
        GetDeviceInventoryRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var query = new DeviceInventoryQuery(
            request.SearchTerm,
            request.CategoryId,
            request.Status,
            request.PageNumber,
            request.PageSize);

        var data = await _repository.GetDevicesAsync(query, cancellationToken);
        if (data is null)
        {
            return null;
        }

        var devices = data.Items
            .Select(device => new DeviceInventoryItemDto(
                device.DeviceId,
                device.DeviceName,
                device.CategoryName,
                device.SerialNumber,
                DeviceBusinessRules.ToLocalizedStatus(device.Status),
                DeviceBusinessRules.BuildDescription(device)))
            .ToList();

        return new GetDeviceInventoryResultDto
        {
            Devices = devices,
            TotalCount = data.TotalCount,
            TotalPages = Math.Max(data.TotalPages, 1)
        };
    }
}

