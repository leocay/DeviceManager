namespace DeviceManagerFE.Features.Devices.Presentation.ViewModels;

public sealed record FilterOptionViewModel(string Value, string Label);

public sealed record DeviceRowViewModel(
    int DeviceId,
    string DeviceCode,
    string DeviceName,
    string CategoryName,
    string? EmployeeName,
    string? SerialNumber,
    string Description,
    string StatusLabel,
    string StatusCssClass);

public sealed class DeviceInventoryViewModel
{
    public IReadOnlyList<DeviceRowViewModel> Devices { get; init; } = [];
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
}

