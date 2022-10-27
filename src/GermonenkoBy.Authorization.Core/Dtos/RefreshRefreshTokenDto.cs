namespace GermonenkoBy.Authorization.Core.Dtos;

public class RefreshRefreshTokenDto
{
    public string Token { get; set; }

    public DateTime? ExpireDate { get; set; }
}