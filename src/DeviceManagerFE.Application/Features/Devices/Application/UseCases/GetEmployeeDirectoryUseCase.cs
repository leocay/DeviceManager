using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;

namespace DeviceManagerFE.Features.Devices.Application.UseCases;

public sealed class GetEmployeeDirectoryUseCase : IGetEmployeeDirectoryUseCase
{
    private readonly IEmployeeDirectoryRepository _employeeDirectoryRepository;

    public GetEmployeeDirectoryUseCase(IEmployeeDirectoryRepository employeeDirectoryRepository)
    {
        _employeeDirectoryRepository = employeeDirectoryRepository;
    }

    public Task<IReadOnlyList<EmployeeOptionDto>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _employeeDirectoryRepository.GetEmployeesAsync(cancellationToken);
}
