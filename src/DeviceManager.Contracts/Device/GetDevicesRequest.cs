namespace DeviceManager.Contracts.Device;

public class GetDevicesRequest
{
    public string? SearchTerm { get; init; }
    public string? CategoryId { get; init; }
    public string? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
