using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Gateway.Api.Models.Contacts;

public class ContactEmailAddress
{
    public int ContactId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Адрес электронной почты – обязательное поле.")]
    [StringLength(250, ErrorMessage = "Максимальная длина адреса электронной почты – 250 символов.")]
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты.")]
    public string Email { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime LastUsedDate { get; set; }
}