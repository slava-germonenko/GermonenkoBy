namespace GermonenkoBy.Authorization.Core.Dtos;

public class StartUserSessionDto
{
    public int UserId { get; set; }

    public DateTime ExpireDate { get; set; }

    public Guid DeviceId { get; set; }

    public string DeviceName { get; set; } = string.Empty;
}