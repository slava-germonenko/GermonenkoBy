using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Users.Api.Models;

public class PasswordRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль – обязательное поле.")]
    public string Password { get; set; } = string.Empty;
}