using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Common.Domain.Tests;

public class TestEntity
{
    [Required(ErrorMessage = ErrorMessages.Required)]
    [EmailAddress(ErrorMessage = ErrorMessages.EmailAddress)]
    [StringLength(10, ErrorMessage = ErrorMessages.StringLength)]
    public string EmailAddress { get; set; } = string.Empty;
}