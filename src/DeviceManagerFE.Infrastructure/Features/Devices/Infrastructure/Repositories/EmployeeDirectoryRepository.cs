using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Application.Interfaces;
using DeviceManagerFE.Services;

namespace DeviceManagerFE.Features.Devices.Infrastructure.Repositories;

public sealed class EmployeeDirectoryRepository : IEmployeeDirectoryRepository
{
    private readonly DeviceApiClient _deviceApiClient;

    public EmployeeDirectoryRepository(DeviceApiClient deviceApiClient)
    {
        _deviceApiClient = deviceApiClient;
    }

    public Task<IReadOnlyList<EmployeeOptionDto>> GetEmployeesAsync(CancellationToken cancellationToken = default)
        => _deviceApiClient.GetEmployeesAsync(cancellationToken);
}
