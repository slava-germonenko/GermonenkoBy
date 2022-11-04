namespace GermonenkoBy.Gateway.Api.Models.Users;

public class User
{
    public int Id { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public bool Active { get; set; } = true;
}