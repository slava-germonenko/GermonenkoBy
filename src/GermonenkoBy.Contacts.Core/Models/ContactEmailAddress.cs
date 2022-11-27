using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Contacts.Core.Models;

public class ContactEmailAddress
{
    public int Id { get; set; }

    public int ContactId { get; set; }

    [Required(ErrorMessage = "Адрес электронной почты – обязательное поле.")]
    public required string Address { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime LastUsedDate { get; set; }
}