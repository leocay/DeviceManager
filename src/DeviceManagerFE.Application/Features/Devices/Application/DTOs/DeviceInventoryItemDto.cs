namespace DeviceManagerFE.Features.Devices.Application.DTOs;

public sealed record DeviceInventoryItemDto(
    int DeviceId,
    string DeviceCode,
    string DeviceName,
    string CategoryName,
    string? EmployeeName,
    string? SerialNumber,
    string Status,
    string Description);

