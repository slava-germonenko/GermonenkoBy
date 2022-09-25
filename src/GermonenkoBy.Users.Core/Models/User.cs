using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Users.Core.Models;

public class User : IChangeDateTrackingModel
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    public string PasswordHash { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;
}