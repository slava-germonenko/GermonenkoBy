using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Contacts.Core.Dtos;
using GermonenkoBy.Contacts.Core.Models;
using GermonenkoBy.Contacts.Infrastructure;
using GermonenkoBy.Contacts.Infrastructure.Repos;
using Contact = GermonenkoBy.Contacts.Infrastructure.Entities.Contact;

namespace GermonenkoBy.Contacts.Tests.Repos;

[TestClass]
public class ContactsRepositoryTests
{
    [TestMethod]
    public async Task GetContactById_Should_ReturnContact()
    {
        var context = CreateContext("GetContactById_Should_ReturnContact");
        var repository = new ContactsRepository(context);

        var now = DateTime.UtcNow;
        var testContact = new Contact
        {
            FirstName = "Darth",
            LastName = "Vader",
            PhoneNumber = "123456789",
            EmailAddresses = new()
            {
                new()
                {
                    Email = "test1@test.com",
                    CreatedDate = now,
                    LastUsedDate = now,
                }
            }
        };

        context.Contacts.Add(testContact);
        await context.SaveChangesAsync();

        var contact = await repository.GetContactAsync(testContact.Id);
        Assert.IsNotNull(contact);
        Assert.AreEqual(testContact.FirstName, contact.FirstName);
        Assert.AreEqual(testContact.LastName, contact.LastName);
        Assert.AreEqual(testContact.PhoneNumber, contact.PhoneNumber);
        Assert.IsTrue(contact.EmailAddresses.Any());
    }

    [TestMethod]
    public async Task GetContactByIdThatDoesNotExist_ShouldNot_ThrowNotFoundException()
    {
        var context = CreateContext("GetContactByIdThatDoesNotExist_ShouldNot_ThrowNotFoundException");
        var repository = new ContactsRepository(context);

        var contact = await repository.GetContactAsync(1);
        Assert.IsNull(contact);
    }

    [TestMethod]
    public async Task SaveNewContact_Should_AddNewContact()
    {
        var context = CreateContext("SaveNewContact_Should_AddNewContact");
        var repository = new ContactsRepository(context);

        var contact = new Contact
        {
            PhoneNumber = "123456789",
            FirstName = "Darth",
            LastName = "Vader",
        };

        var savedContact = await repository.SaveContactAsync(contact);
        Assert.AreEqual(savedContact.PhoneNumber, contact.PhoneNumber);
        Assert.AreEqual(savedContact.FirstName, contact.FirstName);
        Assert.AreEqual(savedContact.LastName, contact.LastName);
    }

    [TestMethod]
    public async Task SavePreviouslyCreatedContact_Should_UpdateExistingContact()
    {
        var context = CreateContext("SavePreviouslyCreatedContact_Should_UpdateExistingContact");
        var repository = new ContactsRepository(context);

        var testContact = new Contact
        {
            PhoneNumber = "123456789",
            FirstName = "Darth",
            LastName = "Vader",
        };

        context.Contacts.Add(testContact);
        await context.SaveChangesAsync();

        var contact = new Contact
        {
            Id = testContact.Id,
            PhoneNumber = "987654321",
            FirstName = "Darth2",
            LastName = "Vader2",
        };

        var savedContact = await repository.SaveContactAsync(contact);
        Assert.AreEqual(testContact.Id, savedContact.Id);
        Assert.AreEqual(savedContact.PhoneNumber, contact.PhoneNumber);
        Assert.AreEqual(savedContact.FirstName, contact.FirstName);
        Assert.AreEqual(savedContact.LastName, contact.LastName);
    }

    [TestMethod]
    public async Task CreateContactWithUsedPhoneNumber_Should_ThrowInfrastructureException()
    {
        var context = CreateContext("CreateContactWithUsedPhoneNumber_Should_ThrowInfrastructureException");
        var repository = new ContactsRepository(context);

        const string phone = "123456789";

        var testContact = new Contact
        {
            PhoneNumber = phone,
            FirstName = "Darth",
            LastName = "Vader",
        };

        context.Contacts.Add(testContact);
        await context.SaveChangesAsync();

        await Assert.ThrowsExceptionAsync<InfrastructureException>(async () =>
        {
            await repository.SaveContactAsync(new()
            {
                PhoneNumber = phone,
                FirstName = "Darth",
                LastName = "Vader",
            });
        });
    }

    [TestMethod]
    public async Task AddContactEmails_Should_AddOnlyNewUniqueEmails()
    {
        var context = CreateContext("AddContactEmails_Should_AddOnlyNewUniqueEmails");
        var repository = new ContactsRepository(context);

        var testContact = new Contact
        {
            PhoneNumber = "123456789",
            FirstName = "Darth",
            LastName = "Vader",
            EmailAddresses = new()
            {
                new ()
                {
                    Email = "test1@test.com",
                }
            }
        };

        context.Contacts.Add(testContact);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var newEmail = "test2@test.com";
        var duplicateEmail = "test1@test.com";

        await repository.AddContactEmails(testContact.Id, new[] { newEmail, duplicateEmail });

        var emails = await context.ContactEmailAddresses.ToListAsync();
        Assert.AreEqual(2, emails.Count);
        Assert.IsTrue(emails.Any(e => e.Email.Equals(newEmail)));
        Assert.IsTrue(emails.Any(e => e.Email.Equals(duplicateEmail)));
    }

    [TestMethod]
    public async Task AddContactEmailsToMissingContact_Should_ThrowInfrastructureException()
    {
        var context = CreateContext("AddContactEmailsToMissingContact_Should_ThrowInfrastructureException");
        var repository = new ContactsRepository(context);

        await Assert.ThrowsExceptionAsync<InfrastructureException>(async () =>
        {
            await repository.AddContactEmails(1, new[] { "test@test.com" });
        });
    }

    [TestMethod]
    public async Task RemoveContactEmails_Should_RemovePassedEmails()
    {
        var context = CreateContext("RemoveContactEmails_Should_RemovePassedEmails");
        var repository = new ContactsRepository(context);

        var contact = new Contact
        {
            PhoneNumber = "12345678",
            FirstName = "Darth",
            LastName = "Vader",
            EmailAddresses = new List<ContactEmailAddress>
            {
                new()
                {
                    Email = "test1@test.com"
                },
                new()
                {
                    Email = "test2@test.com"
                }
            }
        };

        context.Contacts.Add(contact);
        await context.SaveChangesAsync();

        await repository.RemoveContactEmails(contact.Id, new[] { "test1@test.com" });

        var emails = await context.ContactEmailAddresses.ToListAsync();
        Assert.AreEqual(1, emails.Count);
        Assert.AreEqual("test2@test.com", emails.First().Email);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("12345678")]
    public async Task FilterContactsByPhone_Should_ReturnCorrectlyFilteredContacts(string? phone)
    {
        var context = CreateContext("GetListOfContacts_Should_ReturnCorrectlyFilteredContacts" + (phone ?? ""));
        var repository = new ContactsRepository(context);

        context.Contacts.AddRange(
            new Contact
            {
                FirstName = "FN1",
                LastName = "LN2",
                PhoneNumber = "12345678"
            },
            new Contact
            {
                FirstName = "FN1",
                LastName = "LN2",
                PhoneNumber = "87654321"
            }
        );
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var contactsFilter = new ContactsFilterDto
        {
            Count = 5,
            Offset = 0,
            Phone = phone,
        };

        var contactsSet = await repository.GetContactsListAsync(contactsFilter);
        var contactsCount = phone is null ? 2 : 1;
        Assert.AreEqual(contactsCount, contactsSet.Data.Count);
        Assert.AreEqual(contactsCount, contactsSet.Total);
    }

    private static ContactsContext CreateContext(string contextName)
    {
        var contextOptions = new DbContextOptionsBuilder<ContactsContext>()
            .UseInMemoryDatabase(contextName)
            .Options;

        return new ContactsContext(contextOptions);
    }
}