namespace DeviceManager.Domain.Entities;

public class Admin
{
    public int AdminId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
