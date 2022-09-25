using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GermonenkoBy.Common.Utils.Hashing;

public class Pbkdf2Hasher : IHasher
{
    private int BytesLenght => _hashingOptions.PasswordHashBytesLenght;

    private int DefaultSaltLength => _hashingOptions.DefaultSaltLenght;

    private int IterationsCount => _hashingOptions.PasswordHashIterationsCount;

    private readonly HashingOptions _hashingOptions;

    public Pbkdf2Hasher(HashingOptions hashingOptions)
    {
        _hashingOptions = hashingOptions;
    }

    public string GetHash(string source, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        var hash = KeyDerivation.Pbkdf2(
            source,
            saltBytes,
            KeyDerivationPrf.HMACSHA256,
            IterationsCount,
            BytesLenght
        );
        return Convert.ToBase64String(hash);
    }

    public (string hash, string salt) GetHash(string source)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(DefaultSaltLength);
        var hash = KeyDerivation.Pbkdf2(
            source,
            saltBytes,
            KeyDerivationPrf.HMACSHA256,
            IterationsCount,
            BytesLenght
        );
        return (
            Convert.ToBase64String(hash),
            Convert.ToBase64String(saltBytes)
        );
    }
}