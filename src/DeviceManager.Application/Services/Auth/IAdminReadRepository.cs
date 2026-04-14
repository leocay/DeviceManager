using DeviceManager.Domain.Entities;

namespace DeviceManager.Application.Services.Auth;

public interface IAdminReadRepository
{
    Task<Admin?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
}