namespace DeviceManagerBE.Contracts.Device;

public sealed class UpdateDeviceResponse
{
    public int DeviceId { get; init; }
    public string DeviceCode { get; init; } = string.Empty;
    public string DeviceName { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}
