namespace GermonenkoBy.Authorization.Core.Dtos;

public class RefreshRefreshTokenDto
{
    public string Token { get; set; } = string.Empty;

    public DateTime? ExpireDate { get; set; }
}