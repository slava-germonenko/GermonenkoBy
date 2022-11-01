namespace GermonenkoBy.Gateway.Api.Models.Auth;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;

    public Guid UserSessionId { get; set; }

    public DateTime ExpireDate { get; set; }
}