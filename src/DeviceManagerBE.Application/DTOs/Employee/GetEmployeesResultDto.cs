namespace DeviceManagerBE.Application.DTOs.Employee;

public sealed class GetEmployeesResultDto
{
    public IReadOnlyList<EmployeeOptionResultDto> Employees { get; init; } = [];
}
