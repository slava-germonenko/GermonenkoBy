using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Authorization.Core.Models;

public class RefreshToken
{
    [Key]
    [StringLength(400, ErrorMessage = "Максимальная длина токена – 400 символов.")]
    public string Token { get; set; } = string.Empty;

    public Guid UserSessionId { get; set; }

    public DateTime ExpireDate { get; set; }
}