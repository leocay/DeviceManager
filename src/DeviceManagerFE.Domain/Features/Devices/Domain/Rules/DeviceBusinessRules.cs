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

        return "Thiáº¿t bá»‹ cÃ´ng nghá»‡";
    }

    public static string ToLocalizedStatus(string? status) => status switch
    {
        "Available" => "Má»›i",
        "In Use" => "Äang sá»­ dá»¥ng",
        "Maintenance" => "Báº£o trÃ¬",
        "Reserved" => "ÄÃ£ Ä‘áº·t trÆ°á»›c",
        "Retired" => "Há»ng",
        _ => string.IsNullOrWhiteSpace(status) ? "KhÃ´ng rÃµ" : status
    };
}

