namespace GermonenkoBy.Gateway.Api.Models.Auth;

public class RefreshRefreshTokenDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime? ExpireDate { get; set; }
}