namespace GermonenkoBy.Gateway.Api.Models.Auth;

public class AccessToken
{
    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpireDate { get; set; }
}