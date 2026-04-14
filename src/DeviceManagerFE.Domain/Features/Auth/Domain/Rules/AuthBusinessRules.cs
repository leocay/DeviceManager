namespace DeviceManagerFE.Features.Auth.Domain.Rules;

public static class AuthBusinessRules
{
    public static string BuildLoginSuccessMessage(string? fullName)
    {
        var displayName = string.IsNullOrWhiteSpace(fullName) ? "Administrator" : fullName;
        return $"Xin chÃ o {displayName}. ÄÄƒng nháº­p thÃ nh cÃ´ng vÃ o há»‡ thá»‘ng.";
    }
}

