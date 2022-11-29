using System.ComponentModel.DataAnnotations;
using GermonenkoBy.Contacts.Infrastructure.Entities;

namespace GermonenkoBy.Contacts.Tests.Models;

[TestClass]
public class ContactValidationTests
{
    [TestMethod]
    [DataRow("", "Vader", "123456789", "test@test.com")]
    [DataRow("Darth", "", "123456789", "test@test.com")]
    [DataRow("Darth", "Vader", "", "test@test.com")]
    [DataRow("Darth", "Vader", "123", "test@test.com")]
    [DataRow("Darth", "Vader", "qqqqqqqqq", "test@test.com")]
    [DataRow("Darth", "Vader", "123456789", "")]
    [DataRow("Darth", "Vader", "123456789", "test")]
    public void ValidateInvalidContactModel_Should_ReturnValidationErrors(
        string firstName,
        string lastName,
        string phoneNumber,
        string emailAddress
    )
    {
        var now = DateTime.UtcNow;
        var contact = new Contact
        {
            AssigneeId = null,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            CreatedDate = now,
            UpdatedDate = now,
            DeletedDate = now,
            LastActivityDate = now,
            EmailAddresses = new()
            {
                new()
                {
                    ContactId = 1,
                    Email = emailAddress,
                    CreatedDate = now,
                    LastUsedDate = now,
                }
            }
        };

        var contactValidationResults = new List<ValidationResult>();
        Validator.TryValidateObject(contact, new ValidationContext(contact), contactValidationResults, true);

        var contactEmailValidationResults = new List<ValidationResult>();
        var emailToValidate = contact.EmailAddresses.First();
        Validator.TryValidateObject(
            emailToValidate,
            new ValidationContext(emailToValidate),
            contactEmailValidationResults,
            true
        );

        Assert.IsTrue(contactValidationResults.Any() || contactEmailValidationResults.Any());
    }
}