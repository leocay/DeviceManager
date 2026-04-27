namespace DeviceManagerFE.Features.Auth.Domain.Rules;

public static class AuthBusinessRules
{
    public static string BuildLoginSuccessMessage(string? fullName)
    {
        var displayName = string.IsNullOrWhiteSpace(fullName) ? "Administrator" : fullName;
        return $"Xin chào {displayName}. Đăng nhập thành công vào hệ thống.";
    }
}

