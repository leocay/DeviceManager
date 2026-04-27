namespace DeviceManagerBE.Application.DTOs.Device;

public sealed class CreateDeviceResultDto
{
    public int DeviceId { get; init; }
    public string DeviceCode { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
}
