namespace GermonenkoBy.Authorization.Core.Models;

public class UserSessions
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public DateTime ExpireDate { get; set; } = DateTime.UtcNow;

    public Guid DeviceId { get; set; }

    public string DeviceName { get; set; } = string.Empty;
}