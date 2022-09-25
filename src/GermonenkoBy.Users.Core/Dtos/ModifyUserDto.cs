using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Users.Core.Dtos;

public class ModifyUserDto
{
    [Required(ErrorMessage = "Имя – обязательное поле.", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Длина имени должна быть более 50 символов.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя – обязательное поле.", AllowEmptyStrings = false)]
    [StringLength(50, ErrorMessage = "Длина фамилии должна быть более 50 символов.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Имя – обязательное поле.", AllowEmptyStrings = false)]
    [StringLength(250, ErrorMessage = "Максимальная длина адреса почты – 250 символов.")]
    public string EmailAddress { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}