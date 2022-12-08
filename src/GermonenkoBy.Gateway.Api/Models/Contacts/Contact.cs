using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Gateway.Api.Models.Contacts;

public class Contact
{
    public int Id { get; set; }

    public int? AssigneeId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Имя – обязательное поле.")]
    [StringLength(50, ErrorMessage = "Максимальная длина имени – 50 символов.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия – обязательное поле.")]
    [StringLength(50, ErrorMessage = "Максимальная длина фамилии – 50 символов.")]
    public string LastName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Номер телефона – обязательное поле.")]
    [StringLength(10, ErrorMessage = "Максимальная длина номера телефона – 10 символов.")]
    [RegularExpression(@"\d{8,10}", ErrorMessage = "Номер телефона должен состоять только из цифр, а также содержать код страны..")]
    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? LastActivityDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public List<ContactEmailAddress> EmailAddresses { get; set; } = new();
}