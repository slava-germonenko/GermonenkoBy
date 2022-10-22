using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Authorization.Core.Models;

public class RefreshToken : IChangeDateTrackingModel
{
    [Key]
    [StringLength(400, ErrorMessage = "Максимальная длина токена – 400 символов.")]
    public string Token { get; set; } = string.Empty;

    public Guid UserSessionId { get; set; }

    public DateTime ExpireDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}