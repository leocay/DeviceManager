using DeviceManagerFE.Features.Auth.Application.DTOs;
using DeviceManagerFE.Features.Auth.Application.Interfaces;
using DeviceManagerFE.Features.Auth.Presentation.Services;
using DeviceManagerFE.Features.Auth.Presentation.ViewModels;
using DeviceManagerFE.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DeviceManagerFE.Components.Pages;

public class HomePageBase : ComponentBase
{
    [Inject] protected ILoginUseCase LoginUseCase { get; set; } = default!;
    [Inject] protected ILoginPresenter LoginPresenter { get; set; } = default!;
    [Inject] protected AuthSessionState AuthSessionState { get; set; } = default!;
    [Inject] protected IHttpContextAccessor HttpContextAccessor { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] protected Microsoft.Extensions.Logging.ILogger<HomePageBase> Logger { get; set; } = default!;

    [SupplyParameterFromForm(FormName = "LoginForm")]
    protected LoginFormViewModel Model { get; set; } = new();

    protected bool Submitting { get; private set; }
    protected string? StatusMessage { get; private set; }
    protected bool IsError { get; private set; }

    protected async Task HandleSubmitAsync()
    {
        Submitting = true;
        StatusMessage = null;

        LoginResultDto result;
        try
        {
            result = await LoginUseCase.ExecuteAsync(new LoginRequestDto(
                Model.Username,
                Model.Password,
                Model.RememberMe));
        }
        catch
        {
            IsError = true;
            StatusMessage = "Không thể kết nối backend. Hãy kiểm tra API đã chạy ở cổng cấu hình.";
            Submitting = false;
            return;
        }

        var status = LoginPresenter.ToStatus(result);
        IsError = status.IsError;
        StatusMessage = status.Message;

        if (!result.Success)
        {
            Submitting = false;
            return;
        }
        AuthSessionState.AccessToken = result.AccessToken;
        AuthSessionState.FullName = result.FullName;
        // persist auth into ISession so refresh keeps user logged in
        try
        {
            var httpContext = HttpContextAccessor.HttpContext;
            httpContext?.Session.SetString("__Auth_AccessToken", result.AccessToken ?? string.Empty);
            httpContext?.Session.SetString("__Auth_FullName", result.FullName ?? string.Empty);

            // also set a persistent cookie as a fallback so full-page reloads send auth info
            try
            {
                var opts = new CookieOptions
                {
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax,
                    Secure = httpContext?.Request.IsHttps ?? false,
                    Expires = DateTimeOffset.UtcNow.AddHours(8)
                };

                if (httpContext?.Response is not null)
                {
                    if (!string.IsNullOrWhiteSpace(result.AccessToken))
                        httpContext.Response.Cookies.Append("__Auth_AccessToken", result.AccessToken, opts);

                    if (!string.IsNullOrWhiteSpace(result.FullName))
                        httpContext.Response.Cookies.Append("__Auth_FullName", result.FullName, opts);
                }
            }
            catch
            {
                // ignore cookie failures
            }
        }
        catch
        {
            // ignore if session unavailable
        }
        // also persist into localStorage so Blazor circuit reload recovers auth
        try
        {
            await JSRuntime.InvokeVoidAsync("blazorSetLocal", "__Auth_AccessToken", result.AccessToken ?? string.Empty);
            await JSRuntime.InvokeVoidAsync("blazorSetLocal", "__Auth_FullName", result.FullName ?? string.Empty);
            Logger.LogInformation("HomePage: requested setting localStorage tokens for user '{Name}'", result.FullName);
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "HomePage: failed to set localStorage tokens via JS interop");
        }
        NavigationManager.NavigateTo("/devices");
    }
}

