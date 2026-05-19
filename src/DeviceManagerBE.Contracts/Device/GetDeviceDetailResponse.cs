namespace DeviceManagerBE.Contracts.Device;

public sealed class GetDeviceDetailResponse
{
    public int DeviceId { get; init; }
    public string DeviceCode { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public int? EmployeeId { get; init; }
    public string? Brand { get; init; }
    public string? Model { get; init; }
    public string? SerialNumber { get; init; }
    public DateTime? PurchaseDate { get; init; }
    public DateTime? WarrantyExpiryDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Note { get; init; }
    public IReadOnlyList<DeviceHistoryResponse> History { get; init; } = [];
}

public sealed class DeviceHistoryResponse
{
    public string ActionType { get; init; } = string.Empty;
    public string ActionBy { get; init; } = string.Empty;
    public DateTime ActionTime { get; init; }
    public string? Content { get; init; }
}
