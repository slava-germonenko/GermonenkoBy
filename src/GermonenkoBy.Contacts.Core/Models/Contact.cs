using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Contacts.Core.Models;

public class Contact : IChangeDateTrackingModel
{
    public int Id { get; set; }

    public int? AssigneeId { get; set; }

    [Required(ErrorMessage = "Имя – обязательное поле.")]
    [StringLength(50, ErrorMessage = "Максимальная длина имени – 50 символов.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Фамилия – обязательное поле.")]
    [StringLength(50, ErrorMessage = "Максимальная длина фамилии – 50 символов.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Номер телефона – обязательное поле.")]
    [StringLength(10, ErrorMessage = "Максимальная длина номера телефона – 10 символов.")]
    [RegularExpression(@"\d{8,10}", ErrorMessage = "Номер телефона – обязательное поле.")]
    public required string PhoneNumber { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? LastActivityDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public required List<ContactEmailAddress> EmailAddresses { get; set; }
}