using DeviceManagerFE.Features.Devices.Application.DTOs;

namespace DeviceManagerFE.Features.Devices.Application.Interfaces;

public interface IGetEmployeeDirectoryUseCase
{
    Task<IReadOnlyList<EmployeeOptionDto>> ExecuteAsync(CancellationToken cancellationToken = default);
}
