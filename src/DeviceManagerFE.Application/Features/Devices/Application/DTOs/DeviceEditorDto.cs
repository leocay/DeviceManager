namespace DeviceManagerFE.Features.Devices.Application.DTOs;

public sealed class DeviceEditorDto
{
    public int DeviceId { get; init; }
    public string DeviceCode { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
    public int CategoryId { get; init; }
    public int? EmployeeId { get; init; }
    public string? Brand { get; init; }
    public string? Model { get; init; }
    public string? SerialNumber { get; init; }
    public DateTime? PurchaseDate { get; init; }
    public DateTime? WarrantyExpiryDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Note { get; init; }
}
