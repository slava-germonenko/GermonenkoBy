using System.Security.Cryptography;
using GermonenkoBy.Authorization.Core.Contracts;

namespace GermonenkoBy.Authorization.Infrastructure.Contracts;

public class RandomHexadecimalStringTokenGenerator : IRefreshTokenGenerator
{
    private const int RefreshTokenBytesLength = 32;

    public string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(RefreshTokenBytesLength);
        return Convert.ToHexString(randomBytes).ToLower();
    }
}