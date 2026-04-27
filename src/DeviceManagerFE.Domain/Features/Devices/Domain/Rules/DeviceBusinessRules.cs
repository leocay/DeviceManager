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

        return "Thiet bi cong nghe";
    }

    public static string ToLocalizedStatus(string? status) => status switch
    {
        "Available" => "Moi",
        "In Use" => "Dang su dung",
        "Maintenance" => "Bao tri",
        "Reserved" => "Da dat truoc",
        "Retired" => "Hong",
        _ => string.IsNullOrWhiteSpace(status) ? "Khong ro" : status
    };
}
