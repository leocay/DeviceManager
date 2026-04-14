namespace DeviceManagerBE.Contracts.Auth;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? AccessToken { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
}
