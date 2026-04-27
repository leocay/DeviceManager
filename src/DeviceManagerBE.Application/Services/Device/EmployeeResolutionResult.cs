namespace DeviceManagerBE.Application.Services.Device;

public sealed class EmployeeResolutionResult
{
    public int? EmployeeId { get; init; }
    public bool IsAmbiguous { get; init; }
}
