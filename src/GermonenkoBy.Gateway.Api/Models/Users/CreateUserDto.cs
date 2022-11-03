using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Gateway.Api.Models.Users;

public class CreateUserDto : ModifyUserDto
{
    [Required(ErrorMessage = "Пароль – обязательное поле.")]
    public string Password { get; set; } = string.Empty;
}