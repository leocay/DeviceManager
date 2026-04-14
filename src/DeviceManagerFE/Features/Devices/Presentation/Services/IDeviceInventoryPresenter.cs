using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Presentation.ViewModels;

namespace DeviceManagerFE.Features.Devices.Presentation.Services;

public interface IDeviceInventoryPresenter
{
    IReadOnlyList<FilterOptionViewModel> CategoryOptions { get; }
    IReadOnlyList<FilterOptionViewModel> StatusOptions { get; }
    DeviceInventoryViewModel ToViewModel(GetDeviceInventoryResultDto source);
}

