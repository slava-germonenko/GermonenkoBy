using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Sessions.Core.Models;

public class UserSession : IChangeDateTrackingModel
{
    [Key]
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public DateTime ExpireDate { get; set; }

    public Guid DeviceId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Название устройства – обязательное поле.")]
    public string DeviceName { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}