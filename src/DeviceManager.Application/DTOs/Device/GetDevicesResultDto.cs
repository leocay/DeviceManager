namespace DeviceManager.Application.DTOs.Device;

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
    public string DeviceName { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string? SerialNumber { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Brand { get; init; }
    public string? Model { get; init; }
}
