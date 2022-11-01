using GermonenkoBy.Gateway.Api.Models.Auth;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IAuthClient
{
    public Task<RefreshToken> AuthorizeAsync(AuthorizeDto authorizeDto);

    public Task<RefreshToken> RefreshRefreshToken(RefreshRefreshTokenDto refreshTokenDto);

    public Task TerminateSessionAsync(string token);
}