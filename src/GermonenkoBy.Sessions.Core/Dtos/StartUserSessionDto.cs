using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Sessions.Core.Dtos;

public class StartUserSessionDto
{
    public int UserId { get; set; }

    public DateTime ExpireDate { get; set; }

    public Guid DeviceId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Название устройства – обязательное поле.")]
    public string DeviceName { get; set; } = string.Empty;
}