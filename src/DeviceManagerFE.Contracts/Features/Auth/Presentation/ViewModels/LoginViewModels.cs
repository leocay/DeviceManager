namespace DeviceManagerFE.Features.Auth.Presentation.ViewModels;

public sealed class LoginFormViewModel
{
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    public string Username { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public sealed class LoginStatusViewModel
{
    public bool IsError { get; init; }
    public string Message { get; init; } = string.Empty;
}

