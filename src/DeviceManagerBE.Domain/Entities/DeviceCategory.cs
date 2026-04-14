namespace DeviceManagerBE.Domain.Entities;

public class DeviceCategory
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Device> Devices { get; set; } = new List<Device>();
}

