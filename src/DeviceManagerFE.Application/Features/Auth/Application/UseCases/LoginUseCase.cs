using DeviceManagerFE.Features.Auth.Application.DTOs;
using DeviceManagerFE.Features.Auth.Application.Interfaces;
using DeviceManagerFE.Features.Auth.Domain.Rules;

namespace DeviceManagerFE.Features.Auth.Application.UseCases;

public sealed class LoginUseCase : ILoginUseCase
{
    private readonly IAuthRepository _authRepository;

    public LoginUseCase(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<LoginResultDto> ExecuteAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var result = await _authRepository.LoginAsync(
            request.Username,
            request.Password,
            request.RememberMe,
            cancellationToken);

        return new LoginResultDto
        {
            Success = result.Success,
            Message = result.Success
                ? AuthBusinessRules.BuildLoginSuccessMessage(result.FullName)
                : result.Message,
            FullName = result.FullName,
            AccessToken = result.AccessToken
        };
    }
}

