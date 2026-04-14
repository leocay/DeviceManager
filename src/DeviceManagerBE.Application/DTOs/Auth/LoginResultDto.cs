namespace DeviceManagerBE.Application.DTOs.Auth;

public sealed class LoginResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public string? ErrorCode { get; set; }
}
