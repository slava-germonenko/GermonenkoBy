using GermonenkoBy.Gateway.Api.Models.Users;

namespace GermonenkoBy.Gateway.Api.Models.Auth;

public class AuthorizationResult
{
    public AccessToken AccessToken { get; }

    public RefreshToken RefreshToken { get; }

    public User User { get; }

    public AuthorizationResult(
        AccessToken accessToken,
        RefreshToken refreshToken, User user)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        User = user;
    }
}