using DeviceManagerBE.Application.Common;
using DeviceManagerBE.Application.DTOs.Auth;
using DeviceManagerBE.Application.Services.Auth;
using MediatR;

namespace DeviceManagerBE.Application.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResultDto>
{
    private readonly IAdminReadRepository _adminReadRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginCommandHandler(
        IAdminReadRepository adminReadRepository,
        IPasswordHasher passwordHasher,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _adminReadRepository = adminReadRepository;
        _passwordHasher = passwordHasher;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var admin = await _adminReadRepository.GetByUsernameAsync(request.Username, cancellationToken);
        if (admin is null)
        {
            return new LoginResultDto
            {
                Success = false,
                Message = "Sai ten dang nhap hoac mat khau.",
                ErrorCode = ErrorCodes.AuthInvalidCredentials
            };
        }

        var passwordHash = _passwordHasher.Hash(request.Password);
        if (!string.Equals(admin.PasswordHash, passwordHash, StringComparison.Ordinal))
        {
            return new LoginResultDto
            {
                Success = false,
                Message = "Sai ten dang nhap hoac mat khau.",
                ErrorCode = ErrorCodes.AuthInvalidCredentials
            };
        }

        var accessToken = _accessTokenGenerator.Generate(admin.AdminId, admin.Username, admin.FullName);

        return new LoginResultDto
        {
            Success = true,
            Message = "Dang nhap thanh cong.",
            FullName = admin.FullName,
            AccessToken = accessToken.Token,
            ExpiresAtUtc = accessToken.ExpiresAtUtc
        };
    }
}
