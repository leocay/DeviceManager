namespace DeviceManagerBE.Domain.Entities;

public class Employee
{
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Department { get; set; }
    public string? Position { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Device> Devices { get; set; } = new List<Device>();
}

