namespace DeviceManagerBE.Contracts.Device;

public class DeviceDto
{
    public int DeviceId { get; init; }
    public string DeviceName { get; init; } = string.Empty;
    public string CategoryName { get; init; } = string.Empty;
    public string? SerialNumber { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Brand { get; init; }
    public string? Model { get; init; }
}

