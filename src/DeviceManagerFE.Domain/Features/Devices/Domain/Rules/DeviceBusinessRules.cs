using DeviceManagerFE.Features.Devices.Domain.Entities;

namespace DeviceManagerFE.Features.Devices.Domain.Rules;

public static class DeviceBusinessRules
{
    public static string BuildDescription(DeviceEntity device)
    {
        if (!string.IsNullOrWhiteSpace(device.Brand) && !string.IsNullOrWhiteSpace(device.Model))
        {
            return $"{device.Brand} {device.Model}";
        }

        return "Thiết bị công nghệ";
    }

    public static string ToLocalizedStatus(string? status) => status switch
    {
        "Available" => "Mới",
        "In Stock" => "Trong Kho",
        "In Use" => "Đang sử dụng",
        "Maintenance" => "Bảo trì",
        "Reserved" => "Đã đặt trước",
        "Retired" => "Hỏng",
        _ => string.IsNullOrWhiteSpace(status) ? "Không rõ" : status
    };
}
