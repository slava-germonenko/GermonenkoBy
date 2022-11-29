using Moq;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Contacts.Core.Contracts;
using GermonenkoBy.Contacts.Core.Dtos;
using GermonenkoBy.Contacts.Core.Models;
using GermonenkoBy.Contacts.Core.Services;

namespace GermonenkoBy.Contacts.Tests.Services;

[TestClass]
public class ContactsServiceTests
{
    private readonly Mock<IContactsRepository> _contactsRepoMock;

    private readonly Mock<IUsersRepository> _usersRepoMock;

    private readonly ContactsService _contactsService;

    public ContactsServiceTests()
    {
        _contactsRepoMock = new Mock<IContactsRepository>();
        _usersRepoMock = new Mock<IUsersRepository>();
        _contactsService = new ContactsService(
            _contactsRepoMock.Object,
            _usersRepoMock.Object
        );
    }

    [TestMethod]
    public async Task CreateUserWithUsedPhoneNumber_Should_ThrowException()
    {
        const string duplicatePhone = "12345678";
        var contactDto = new CreateContactDto
        {
            PhoneNumber = duplicatePhone,
        };

        var contactsSet = new PagedSet<Contact>
        {
            Data = new List<Contact>
            {
                new()
                {
                    PhoneNumber = duplicatePhone
                }
            }
        };
        _contactsRepoMock.Setup(
                repo => repo.GetContactsListAsync(
                    It.Is<ContactsFilterDto>(filter => filter.Phone == duplicatePhone)
                )
            )
            .ReturnsAsync(contactsSet);

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await _contactsService.CreateContactAsync(contactDto);
        });
    }

    [TestMethod]
    public async Task CreateUserWithInvalidAssignee_Should_ThrowException()
    {
        const int invalidAssigneeId = 1;
        var contactDto = new CreateContactDto
        {
            AssigneeId = invalidAssigneeId,
            PhoneNumber = "12345678",
        };

        _contactsRepoMock
            .Setup(repo => repo.GetContactsListAsync(It.IsAny<ContactsFilterDto>()))
            .ReturnsAsync(new PagedSet<Contact>());

        _usersRepoMock.Setup(repo => repo.GetUserAsync(invalidAssigneeId))
            .ReturnsAsync((User?)null);

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await _contactsService.CreateContactAsync(contactDto);
        });
    }

    [TestMethod]
    public async Task CreateUserAndAssignUser_Should_EnsureAssigneeExistsCreateUser()
    {
        const int assigneeId = 1;
        const string phone = "12345678";

        var contactDto = new CreateContactDto
        {
            AssigneeId = assigneeId,
            FirstName = "FL",
            LastName = "LN",
            PhoneNumber = phone,
            MarkAsActivity = true,
            EmailAddresses = new()
            {
                "test@test.com",
            }
        };

        _usersRepoMock
            .Setup(repo => repo.GetUserAsync(assigneeId))
            .ReturnsAsync(new User());

        _contactsRepoMock
            .Setup(repo => repo.GetContactsListAsync(It.IsAny<ContactsFilterDto>()))
            .ReturnsAsync(new PagedSet<Contact>());

        _contactsRepoMock
            .Setup(
                repo => repo.SaveContactAsync(
                    It.Is<Contact>(
                        c => c.FirstName == contactDto.FirstName
                             && c.LastName == contactDto.LastName
                             && c.PhoneNumber == contactDto.PhoneNumber
                             && c.LastActivityDate != null
                    )
                )
            )
            .ReturnsAsync(new Contact{Id = 1});

        _contactsRepoMock
            .Setup(repo => repo.AddContactEmails(1, contactDto.EmailAddresses))
            .Returns(Task.CompletedTask);

        _contactsRepoMock.Setup(repo => repo.GetContactAsync(1))
            .ReturnsAsync(new Contact());

        await _contactsService.CreateContactAsync(contactDto);

        _usersRepoMock.VerifyAll();
        _contactsRepoMock.VerifyAll();
    }
}