namespace DeviceManagerFE.Features.Devices.Application.DTOs;

public sealed class GetDeviceInventoryResultDto
{
    public IReadOnlyList<DeviceInventoryItemDto> Devices { get; init; } = [];
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}

