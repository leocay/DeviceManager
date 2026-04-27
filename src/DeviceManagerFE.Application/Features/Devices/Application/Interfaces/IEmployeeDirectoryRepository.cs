using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IEmployeeDirectoryRepository
{
    Task<IReadOnlyList<EmployeeOptionDto>> GetEmployeesAsync(CancellationToken cancellationToken = default);
}
