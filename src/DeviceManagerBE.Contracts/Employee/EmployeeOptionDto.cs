namespace DeviceManagerBE.Contracts.Employee;

public sealed class EmployeeOptionDto
{
    public int EmployeeId { get; init; }
    public string EmployeeCode { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Department { get; init; }
    public string DisplayName { get; init; } = string.Empty;
}
