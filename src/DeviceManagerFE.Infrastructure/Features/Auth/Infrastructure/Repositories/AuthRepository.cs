using DeviceManagerBE.Contracts.Auth;
using DeviceManagerFE.Features.Auth.Application.Interfaces;
using DeviceManagerFE.Features.Auth.Domain.Entities;
using DeviceManagerFE.Services;

namespace DeviceManagerFE.Features.Auth.Infrastructure.Repositories;

public sealed class AuthRepository : IAuthRepository
{
    private readonly AuthApiClient _authApiClient;

    public AuthRepository(AuthApiClient authApiClient)
    {
        _authApiClient = authApiClient;
    }

    public async Task<AuthenticatedAdminEntity> LoginAsync(
        string username,
        string password,
        bool rememberMe,
        CancellationToken cancellationToken = default)
    {
        var response = await _authApiClient.LoginAsync(new LoginRequest
        {
            Username = username,
            Password = password,
            RememberMe = rememberMe
        }, cancellationToken);

        return new AuthenticatedAdminEntity
        {
            Success = response.Success,
            Message = response.Message,
            FullName = response.FullName,
            AccessToken = response.AccessToken
        };
    }
}


