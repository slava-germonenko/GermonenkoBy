using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Users.Core.Dtos;

public class CreateUserDto : ModifyUserDto
{
    [Required(ErrorMessage = "Пароль – обязательное поле.")]
    public string Password { get; set; } = string.Empty;
}