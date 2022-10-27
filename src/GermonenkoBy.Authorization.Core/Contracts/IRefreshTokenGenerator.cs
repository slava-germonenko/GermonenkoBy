namespace GermonenkoBy.Authorization.Core.Contracts;

public interface IRefreshTokenGenerator
{
    public string GenerateRefreshToken();
}