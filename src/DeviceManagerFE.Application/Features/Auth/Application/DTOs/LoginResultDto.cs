namespace DeviceManagerFE.Features.Auth.Application.DTOs;

public sealed class LoginResultDto
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public string? AccessToken { get; init; }
}

