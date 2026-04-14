using DeviceManagerFE.Features.Auth.Application.DTOs;

namespace DeviceManagerFE.Features.Auth.Application.Interfaces;

public interface ILoginUseCase
{
    Task<LoginResultDto> ExecuteAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default);
}

