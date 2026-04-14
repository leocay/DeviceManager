using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Presentation.ViewModels;

namespace DeviceManagerFE.Features.Devices.Presentation.Services;

public sealed class DeviceInventoryPresenter : IDeviceInventoryPresenter
{
    private static readonly IReadOnlyList<FilterOptionViewModel> CategoryFilterOptions =
    [
        new(string.Empty, "Táº¥t cáº£"),
        new("1", "Laptop"),
        new("2", "Desktop"),
        new("3", "Monitor"),
        new("4", "Printer"),
        new("5", "Scanner"),
        new("6", "Projector"),
        new("7", "Network Switch"),
        new("8", "Router"),
        new("9", "WiFi Access Point"),
        new("10", "UPS"),
        new("11", "Server"),
        new("12", "Keyboard"),
        new("13", "Mouse"),
        new("14", "Docking Station"),
        new("15", "Tablet"),
        new("16", "Smartphone"),
        new("17", "Headset"),
        new("18", "Webcam"),
        new("19", "SSD"),
        new("20", "HDD")
    ];

    private static readonly IReadOnlyList<FilterOptionViewModel> StatusFilterOptions =
    [
        new(string.Empty, "Táº¥t cáº£"),
        new("Available", "Má»›i"),
        new("In Use", "Äang sá»­ dá»¥ng"),
        new("Maintenance", "Báº£o trÃ¬"),
        new("Reserved", "ÄÃ£ Ä‘áº·t trÆ°á»›c"),
        new("Retired", "Há»ng")
    ];

    public IReadOnlyList<FilterOptionViewModel> CategoryOptions => CategoryFilterOptions;
    public IReadOnlyList<FilterOptionViewModel> StatusOptions => StatusFilterOptions;

    public DeviceInventoryViewModel ToViewModel(GetDeviceInventoryResultDto source)
    {
        var rows = source.Devices
            .Select(device => new DeviceRowViewModel(
                device.DeviceId,
                device.DeviceName,
                device.CategoryName,
                device.SerialNumber,
                device.Description,
                device.Status,
                ToStatusCssClass(device.Status)))
            .ToList();

        return new DeviceInventoryViewModel
        {
            Devices = rows,
            TotalCount = source.TotalCount,
            TotalPages = source.TotalPages
        };
    }

    private static string ToStatusCssClass(string statusLabel) => statusLabel switch
    {
        "Má»›i" => "is-new",
        "Äang sá»­ dá»¥ng" => "is-active",
        "Báº£o trÃ¬" => "is-maintenance",
        "ÄÃ£ Ä‘áº·t trÆ°á»›c" => "is-reserved",
        "Há»ng" => "is-retired",
        _ => "is-default"
    };
}

