using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Gateway.Api.Models.Users;

public class PasswordRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль – обязательное поле.")]
    public string Password { get; set; } = string.Empty;
}