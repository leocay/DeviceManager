namespace DeviceManagerBE.Application.DTOs.Employee;

public sealed class EmployeeOptionResultDto
{
    public int EmployeeId { get; init; }
    public string EmployeeCode { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Department { get; init; }
}
