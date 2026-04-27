namespace DeviceManagerBE.Contracts.Employee;

public sealed class GetEmployeesResponse
{
    public List<EmployeeOptionDto> Employees { get; init; } = [];
}
