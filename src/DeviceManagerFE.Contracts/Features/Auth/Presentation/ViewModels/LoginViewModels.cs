namespace DeviceManagerFE.Features.Auth.Presentation.ViewModels;

public sealed class LoginFormViewModel
{
    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "TÃªn Ä‘Äƒng nháº­p lÃ  báº¯t buá»™c.")]
    public string Username { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Máº­t kháº©u lÃ  báº¯t buá»™c.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public sealed class LoginStatusViewModel
{
    public bool IsError { get; init; }
    public string Message { get; init; } = string.Empty;
}

