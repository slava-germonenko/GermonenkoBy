using GermonenkoBy.Contacts.Core.Models;
using Moq;

using GermonenkoBy.Contacts.Infrastructure.Clients;
using GermonenkoBy.Contacts.Infrastructure.Repos;

namespace GermonenkoBy.Contacts.Tests.Repos;

[TestClass]
public class UsersRepositoryTests
{
    private readonly UsersRepository _usersRepository;

    private readonly Mock<IUsersClient> _usersClientMock;

    public UsersRepositoryTests()
    {
        _usersClientMock = new ();
        _usersRepository = new UsersRepository(_usersClientMock.Object);
    }

    [TestMethod]
    public async Task GetUserById_Should_CallGetUser()
    {
        const int userId = 1;

        _usersClientMock
            .Setup(client => client.GetUserAsync(userId))
            .ReturnsAsync(new User
            {
                Id = userId,
            });

        var user = await _usersRepository.GetUserAsync(userId);

        Assert.IsNotNull(user);
        Assert.AreEqual(userId, user.Id);

        _usersClientMock.Verify(client => client.GetUserAsync(userId), Times.Once);
    }

    [TestMethod]
    public async Task GetUserByIdThatDoesNotExist_Should_ReturnNull()
    {
        const int userId = 1;
        _usersClientMock
            .Setup(client => client.GetUserAsync(userId))
            .ReturnsAsync((User?)null);

        var user = await _usersRepository.GetUserAsync(userId);

        Assert.IsNull(user);

        _usersClientMock.Verify(client => client.GetUserAsync(userId), Times.Once);
    }
}