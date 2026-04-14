namespace DeviceManager.Contracts.Auth;

public class LoginRequest
{
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MaxLength(200)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}