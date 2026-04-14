using DeviceManagerBE.Application.DTOs.Auth;
using MediatR;

namespace DeviceManagerBE.Application.Features.Auth.Login;

public sealed class LoginCommand : IRequest<LoginResultDto>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public bool RememberMe { get; init; }
}
