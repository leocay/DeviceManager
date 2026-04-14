using DeviceManagerFE.Features.Auth.Domain.Entities;

namespace DeviceManagerFE.Features.Auth.Application.Interfaces;

public interface IAuthRepository
{
    Task<AuthenticatedAdminEntity> LoginAsync(
        string username,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken = default);
}

