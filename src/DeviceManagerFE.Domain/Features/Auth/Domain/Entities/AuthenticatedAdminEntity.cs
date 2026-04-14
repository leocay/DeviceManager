namespace DeviceManagerFE.Features.Auth.Domain.Entities;

public sealed class AuthenticatedAdminEntity
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? FullName { get; init; }
    public string? AccessToken { get; init; }
}

