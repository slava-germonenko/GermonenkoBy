namespace GermonenkoBy.Authorization.Core.Models;

public class User
{
    public int Id { get; set; }

    public bool Active { get; set; }

    public string EmailAddress { get; set; } = string.Empty;
}