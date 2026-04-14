namespace DeviceManagerFE.Features.Devices.Application.DTOs;

public sealed record DeviceInventoryItemDto(
    int DeviceId,
    string DeviceName,
    string CategoryName,
    string? SerialNumber,
    string Status,
    string Description);

