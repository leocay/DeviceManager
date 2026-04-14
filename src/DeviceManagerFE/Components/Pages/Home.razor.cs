using DeviceManagerFE.Features.Auth.Application.DTOs;
using DeviceManagerFE.Features.Auth.Application.Interfaces;
using DeviceManagerFE.Features.Auth.Presentation.Services;
using DeviceManagerFE.Features.Auth.Presentation.ViewModels;
using DeviceManagerFE.Services;
using Microsoft.AspNetCore.Components;

namespace DeviceManagerFE.Components.Pages;

public class HomePageBase : ComponentBase
{
    [Inject] protected ILoginUseCase LoginUseCase { get; set; } = default!;
    [Inject] protected ILoginPresenter LoginPresenter { get; set; } = default!;
    [Inject] protected AuthSessionState AuthSessionState { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

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
            StatusMessage = "KhÃ´ng thá»ƒ káº¿t ná»‘i backend. HÃ£y kiá»ƒm tra API Ä‘Ã£ cháº¡y á»Ÿ cá»•ng cáº¥u hÃ¬nh.";
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
        NavigationManager.NavigateTo("/devices");
    }
}

