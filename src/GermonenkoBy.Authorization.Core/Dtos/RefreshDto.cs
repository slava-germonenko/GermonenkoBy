namespace GermonenkoBy.Authorization.Core.Dtos;

public class RefreshDto
{
    public string Token { get; set; }

    public DateTime? ExpireDate { get; set; }
}