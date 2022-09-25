using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GermonenkoBy.Common.Utils.Hashing;

namespace GermonenkoBy.Common.Utils.Tests;

[TestClass, TestCategory("Hashing")]
public class Pbkdf2HasherTests
{
    private readonly Pbkdf2Hasher _hasher = new(new HashingOptions
    {
        PasswordHashIterationsCount = 100,
        PasswordHashBytesLenght = 32,
        DefaultSaltLenght = 16
    });

    // Should produce the same output whenever same inputs are provided
    [TestMethod]
    public void HashPassword_ShouldBeClean()
    {
        const string testPassword = "12345";
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));

        var hash1 = _hasher.GetHash(testPassword, salt);
        var hash2 = _hasher.GetHash(testPassword, salt);

        Assert.AreEqual(hash1, hash1);
    }

    [TestMethod]
    public void HashPassword_EachTimeShouldGenerateDifferentSalt()
    {
        const string testPassword = "12345";
        var (hash1, salt1) = _hasher.GetHash(testPassword);
        var (hash2, salt2) = _hasher.GetHash(testPassword);

        Assert.AreNotEqual(hash1, hash2);
        Assert.AreNotEqual(salt1, salt2);
    }
}