namespace DeviceManagerFE.Features.Devices.Application.DTOs;

public sealed record CreateDeviceRequestDto(
    string DeviceCode,
    string DeviceName,
    int CategoryId,
    string? Brand,
    string? Model,
    string? SerialNumber,
    DateTime? PurchaseDate,
    DateTime? WarrantyExpiryDate,
    string Status,
    int? EmployeeId,
    string? Note);
