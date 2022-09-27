using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Api.Models;

public class UserViewModel
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    public static UserViewModel FromUser(User user) => new()
    {
        Id = user.Id,
        CreatedDate = user.CreatedDate,
        UpdatedDate = user.UpdatedDate,
        FirstName = user.FirstName,
        LastName = user.LastName,
        EmailAddress = user.EmailAddress,
        Active = user.Active,
    };
}