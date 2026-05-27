using Microsoft.JSInterop;

namespace DeviceManagerFE.Services;

public sealed class BrowserAuthSessionRestorer
{
    private const string AccessTokenKey = "__Auth_AccessToken";
    private const string FullNameKey = "__Auth_FullName";

    private readonly AuthSessionState _authSessionState;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJSRuntime _jsRuntime;

    public BrowserAuthSessionRestorer(
        AuthSessionState authSessionState,
        IHttpContextAccessor httpContextAccessor,
        IJSRuntime jsRuntime)
    {
        _authSessionState = authSessionState;
        _httpContextAccessor = httpContextAccessor;
        _jsRuntime = jsRuntime;
    }

    public bool RestoreFromCurrentRequest()
    {
        if (_authSessionState.IsAuthenticated)
        {
            return true;
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return false;
        }

        string? token = null;
        string? name = null;

        try
        {
            token = httpContext.Session.GetString(AccessTokenKey);
            name = httpContext.Session.GetString(FullNameKey);
        }
        catch
        {
            // Session may be unavailable after the interactive circuit starts.
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            token = httpContext.Request.Cookies[AccessTokenKey];
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            name = httpContext.Request.Cookies[FullNameKey];
        }

        return Apply(token, name);
    }

    public async Task<bool> RestoreFromBrowserAsync()
    {
        if (_authSessionState.IsAuthenticated)
        {
            return true;
        }

        try
        {
            var token = await _jsRuntime.InvokeAsync<string?>("blazorGetLocal", AccessTokenKey);
            var name = await _jsRuntime.InvokeAsync<string?>("blazorGetLocal", FullNameKey);

            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _jsRuntime.InvokeAsync<string?>("blazorGetCookie", AccessTokenKey);
                name = await _jsRuntime.InvokeAsync<string?>("blazorGetCookie", FullNameKey);
            }

            return Apply(token, name);
        }
        catch
        {
            return false;
        }
    }

    private bool Apply(string? token, string? name)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        _authSessionState.AccessToken = token;
        _authSessionState.FullName = string.IsNullOrWhiteSpace(name) ? null : name;
        return true;
    }
}
