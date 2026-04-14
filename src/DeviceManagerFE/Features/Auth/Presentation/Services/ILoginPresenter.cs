using DeviceManagerFE.Features.Auth.Application.DTOs;
using DeviceManagerFE.Features.Auth.Presentation.ViewModels;

namespace DeviceManagerFE.Features.Auth.Presentation.Services;

public interface ILoginPresenter
{
    LoginStatusViewModel ToStatus(LoginResultDto source);
}

