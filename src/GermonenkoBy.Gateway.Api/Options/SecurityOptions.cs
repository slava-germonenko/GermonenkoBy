namespace GermonenkoBy.Gateway.Api.Options;

public class SecurityOptions
{
    public int AccessTokenTtlSeconds { get; set; }

    public string JwtSecret { get; set; } = string.Empty;
}