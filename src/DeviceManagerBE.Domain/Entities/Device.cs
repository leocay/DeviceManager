namespace DeviceManagerBE.Domain.Entities;

public class Device
{
    public int DeviceId { get; set; }
    public string DeviceCode { get; set; } = null!;
    public string DeviceName { get; set; } = null!;
    public int CategoryId { get; set; }
    public int? EmployeeId { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? WarrantyExpiryDate { get; set; }
    public string Status { get; set; } = null!;
    public int Quantity { get; set; }
    public string? Location { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public DeviceCategory Category { get; set; } = null!;
    public Employee? Employee { get; set; }
    public ICollection<DeviceLog> DeviceLogs { get; set; } = new List<DeviceLog>();
}

