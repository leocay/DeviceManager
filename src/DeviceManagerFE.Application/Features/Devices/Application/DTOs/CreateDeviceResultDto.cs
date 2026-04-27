namespace DeviceManagerFE.Features.Devices.Application.DTOs;

public sealed class CreateDeviceResultDto
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public int? DeviceId { get; init; }
    public IReadOnlyDictionary<string, string[]> ValidationErrors { get; init; } =
        new Dictionary<string, string[]>(StringComparer.Ordinal);
}
