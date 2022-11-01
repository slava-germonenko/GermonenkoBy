using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Gateway.Api.Models.Auth;

public class AuthorizeDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Логин – обязательное поле.")]
    public string Login { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Пароль – обязательное поле.")]
    public string Password { get; set; } = string.Empty;

    public Guid DeviceId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Название устройства – обязательное поле.")]
    public string DeviceName { get; set; } = string.Empty;
}