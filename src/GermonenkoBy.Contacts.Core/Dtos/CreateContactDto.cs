using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Contacts.Core.Dtos;

public class CreateContactDto
{
    public int? AssigneeId { get; set; } = null;

    [Required(ErrorMessage = "Имя – обязательное поле.")]
    [StringLength(50, ErrorMessage = "Максимальная длина имени – 50 символов.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Фамилия – обязательное поле.")]
    [StringLength(50, ErrorMessage = "Максимальная длина фамилии – 50 символов.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Номер телефона – обязательное поле.")]
    [StringLength(10, ErrorMessage = "Максимальная длина номера телефона – 10 символов.")]
    [RegularExpression(@"\d{8,10}", ErrorMessage = "Номер телефона – обязательное поле.")]
    public string PhoneNumber { get; set; } = string.Empty;

    public bool MarkAsActivity { get; set; } = false;

    public List<string> EmailAddresses { get; set; } = new();
}