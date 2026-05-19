namespace DeviceManagerBE.Application.DTOs.Device;

public class GetDevicesResultDto
{
    public List<DeviceItemDto> Devices { get; init; } = new();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
}

public class DeviceItemDto
{
    public int DeviceId { get; init; }
    public string DeviceCode { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string? EmployeeName { get; init; }
    public string? SerialNumber { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Brand { get; init; }
    public string? Model { get; init; }
}

public sealed class DeviceDetailDto
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
    public IReadOnlyList<DeviceLogItemDto> Logs { get; init; } = [];
}

public sealed class DeviceLogItemDto
{
    public string ActionType { get; init; } = string.Empty;
    public string ActionBy { get; init; } = string.Empty;
    public DateTime ActionTime { get; init; }
    public string? Content { get; init; }
}

