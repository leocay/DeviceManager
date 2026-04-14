using DeviceManagerBE.Domain.Entities;

namespace DeviceManagerBE.Application.Services.Auth;

public interface IAdminReadRepository
{
    Task<Admin?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}
