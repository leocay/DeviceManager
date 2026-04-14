namespace DeviceManagerBE.Domain.Entities;

public class DeviceLog
{
    public int LogId { get; set; }
    public int DeviceId { get; set; }
    public string ActionType { get; set; } = null!;
    public string ActionBy { get; set; } = null!;
    public DateTime ActionTime { get; set; }
    public string? Content { get; set; }

    public Device Device { get; set; } = null!;
}

