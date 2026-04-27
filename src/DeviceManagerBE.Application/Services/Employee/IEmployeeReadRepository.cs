using DeviceManagerBE.Application.DTOs.Employee;

namespace DeviceManagerBE.Application.Services.Employee;

public interface IEmployeeReadRepository
{
    Task<IReadOnlyList<EmployeeOptionResultDto>> GetEmployeesAsync(CancellationToken cancellationToken = default);
}
