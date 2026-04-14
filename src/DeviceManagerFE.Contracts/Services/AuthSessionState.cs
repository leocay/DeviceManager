namespace DeviceManagerFE.Services;

public class AuthSessionState
{
    public string? AccessToken { get; set; }
    public string? FullName { get; set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken);

    public void Clear()
    {
        AccessToken = null;
        FullName = null;
    }
}

