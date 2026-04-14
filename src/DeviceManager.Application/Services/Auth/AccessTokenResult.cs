namespace DeviceManager.Application.Services.Auth;

public sealed record AccessTokenResult(string Token, DateTime ExpiresAtUtc);