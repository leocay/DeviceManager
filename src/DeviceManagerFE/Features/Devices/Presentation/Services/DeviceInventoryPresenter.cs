using DeviceManagerFE.Features.Devices.Application.DTOs;
using DeviceManagerFE.Features.Devices.Presentation.ViewModels;

namespace DeviceManagerFE.Features.Devices.Presentation.Services;

public sealed class DeviceInventoryPresenter : IDeviceInventoryPresenter
{
    private static readonly IReadOnlyList<FilterOptionViewModel> CategoryFilterOptions =
    [
        new(string.Empty, "Tất cả"),
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
        new(string.Empty, "Tất cả"),
        new("Available", "Mới"),
        new("In Stock", "Trong Kho"),
        new("In Use", "Đang sử dụng"),
        new("Maintenance", "Bảo trì"),
        new("Reserved", "Đã đặt trước"),
        new("Retired", "Hỏng")
    ];

    public IReadOnlyList<FilterOptionViewModel> CategoryOptions => CategoryFilterOptions;
    public IReadOnlyList<FilterOptionViewModel> StatusOptions => StatusFilterOptions;

    public DeviceInventoryViewModel ToViewModel(GetDeviceInventoryResultDto source)
    {
        var rows = source.Devices
            .Select(device => new DeviceRowViewModel(
                device.DeviceId,
                device.DeviceCode,
                device.DeviceName,
                device.CategoryName,
                device.EmployeeName,
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
        "Mới" => "is-new",
        "Trong Kho" => "is-stock",
        "Đang sử dụng" => "is-active",
        "Bảo trì" => "is-maintenance",
        "Đã đặt trước" => "is-reserved",
        "Hỏng" => "is-retired",
        _ => "is-default"
    };
}
