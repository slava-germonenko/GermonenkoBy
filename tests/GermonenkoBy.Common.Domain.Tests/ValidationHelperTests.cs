using GermonenkoBy.Common.Domain.Exceptions;

namespace GermonenkoBy.Common.Domain.Tests;

[TestClass]
public class ValidationHelperTests
{
    [DataTestMethod]
    [DataRow("", ErrorMessages.Required)] // Test [Required]
    [DataRow("123", ErrorMessages.EmailAddress)] // Test [EmailAddress]
    [DataRow("1234567890987654321@test", ErrorMessages.StringLength)] // Test [StringLength]
    public void ValidateEntity_ShouldThrowCoreLogicException(string email, string errorMessage)
    {
        var testEntity = new TestEntity
        {
            EmailAddress = email,
        };

        var err = Assert.ThrowsException<CoreLogicException>(() =>
        {
            CoreValidationHelper.EnsureEntityIsValid(testEntity);
        });

        Assert.IsNotNull(err.Message);
        Assert.AreEqual(errorMessage, err.Message);
    }
}