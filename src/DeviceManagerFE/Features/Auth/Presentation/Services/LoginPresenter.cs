using DeviceManagerFE.Features.Auth.Application.DTOs;
using DeviceManagerFE.Features.Auth.Presentation.ViewModels;

namespace DeviceManagerFE.Features.Auth.Presentation.Services;

public sealed class LoginPresenter : ILoginPresenter
{
    public LoginStatusViewModel ToStatus(LoginResultDto source)
    {
        return new LoginStatusViewModel
        {
            IsError = !source.Success,
            Message = source.Message
        };
    }
}

