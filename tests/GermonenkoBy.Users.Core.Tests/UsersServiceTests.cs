using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore;
using Moq;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Utils.Hashing;
using GermonenkoBy.Users.Core.Contracts;
using GermonenkoBy.Users.Core.Dtos;
using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Core.Tests;

[TestClass]
public class UsersServiceTests
{
    private readonly Mock<IHasher> _hasherMock = new();

    private readonly Mock<IPasswordPolicy> _alwaysCorrectPasswordPolicy = new();

    private readonly Mock<IPasswordPolicy> _alwaysIncorrectPasswordPolicy = new();

    private const int DefaultTestUserId = 1;

    private const string FirstNameRequiredValidationError = "Имя – обязательное поле.";

    private const string FirstNameLenghtValidationError = "Длина имени должна быть более 50 символов.";

    public UsersServiceTests()
    {
        _hasherMock.Setup(hasher => hasher.GetHash(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("12345hashed");

        _hasherMock.Setup(hasher => hasher.GetHash(It.IsAny<string>()))
            .Returns(("12345hashed", "salt"));

        _alwaysCorrectPasswordPolicy.Setup(
                policy => policy.PasswordMeetsPolicyRequirements(
                    It.IsAny<string>()
                )
            )
            .Returns(true);

        _alwaysCorrectPasswordPolicy.Setup(policy => policy.PolicyDescription)
            .Returns("Always correct!");

        _alwaysIncorrectPasswordPolicy.Setup(
                policy => policy.PasswordMeetsPolicyRequirements(
                    It.IsAny<string>()
                )
            )
            .Returns(false);

        _alwaysIncorrectPasswordPolicy.Setup(policy => policy.PolicyDescription)
            .Returns("Always incorrect!");
    }

    [TestMethod]
    public async Task GetUser_ShouldReturnUser()
    {
        var context = CreateInMemoryContext();
        context.Users.Add(new User { Id = DefaultTestUserId });
        await context.SaveChangesAsync();

        var service = new UsersService(_hasherMock.Object, _alwaysCorrectPasswordPolicy.Object, context);
        var user = await service.GetUserAsync(DefaultTestUserId);

        Assert.IsNotNull(user);
        Assert.AreEqual(DefaultTestUserId, user.Id);
    }

    [TestMethod]
    public async Task GetUserThatNotExists_ShouldThrowNotFoundException()
    {
        var context = CreateInMemoryContext();
        context.Users.Add(new User { Id = DefaultTestUserId });
        await context.SaveChangesAsync();
        var service = new UsersService(_hasherMock.Object, _alwaysCorrectPasswordPolicy.Object, context);

        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await service.GetUserAsync(-1);
        });
    }

    [TestMethod]
    public async Task CreateUser_ShouldCreateUser()
    {
        var service = new UsersService(
            _hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        var userDto = GetDefaultCreateUserDto();
        var user = await service.CreateUserAsync(userDto);

        Assert.IsNotNull(user);
        Assert.AreNotEqual(default, user.Id);
        Assert.AreEqual(userDto.FirstName, user.FirstName);
        Assert.AreEqual(userDto.LastName, user.LastName);
        Assert.AreEqual(userDto.EmailAddress, user.EmailAddress);
        Assert.AreEqual(userDto.Active, user.Active);
    }

    [TestMethod]
    public async Task CreateUserWithEmailInuse_ShouldThrowCoreLogicException()
    {
        var service = new UsersService(
            _hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        await service.CreateUserAsync(GetDefaultCreateUserDto());

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.CreateUserAsync(GetDefaultCreateUserDto());
        });
    }

    [TestMethod]
    public async Task CreateInvalidUser_ShouldThrowCoreLogicException()
    {
        var invalidUser1 = GetDefaultCreateUserDto();
        invalidUser1.FirstName = "";

        var invalidUser2 = GetDefaultCreateUserDto();
        invalidUser2.FirstName = Convert.ToBase64String(RandomNumberGenerator.GetBytes(100));

        var service = new UsersService(
            _hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        var firstNameRequiredError = await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.CreateUserAsync(invalidUser1);
        });

        var firstNameLengthError = await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.CreateUserAsync(invalidUser2);
        });

        Assert.AreEqual(FirstNameRequiredValidationError, firstNameRequiredError.Message);
        Assert.AreEqual(FirstNameLenghtValidationError, firstNameLengthError.Message);
    }

    [TestMethod]
    public async Task CreateUserWithInvalidPassword_ShouldThrowCoreLogicException()
    {
        var service = new UsersService(
            _hasherMock.Object,
            _alwaysIncorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        var user = GetDefaultCreateUserDto();
        var err = await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.CreateUserAsync(user);
        });

        Assert.AreEqual(_alwaysIncorrectPasswordPolicy.Object.PolicyDescription, err.Message);
    }

    [TestMethod]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        var service = new UsersService(
            _hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        var user = await service.CreateUserAsync(GetDefaultCreateUserDto());
        var updateUserDto = new ModifyUserDto
        {
            FirstName = "FirstName2",
            LastName = "LastName2",
            Active = false,
            EmailAddress = "test2@test.com",
        };

        var updatedUser = await service.UpdateUserBasicDataAsync(user.Id, updateUserDto);
        Assert.AreEqual(updateUserDto.FirstName, updatedUser.FirstName);
        Assert.AreEqual(updateUserDto.LastName, updatedUser.LastName);
        Assert.AreEqual(updateUserDto.EmailAddress, updatedUser.EmailAddress);
        Assert.AreEqual(updateUserDto.Active, updatedUser.Active);
    }

    [TestMethod]
    public async Task UpdateUserWithInvalidData_ShouldThrowCoreLogicException()
    {
        var service = new UsersService(
            _hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        var user = await service.CreateUserAsync(GetDefaultCreateUserDto());
        var updateUserDto = new ModifyUserDto
        {
            FirstName = "",
            LastName = "LastName2",
            Active = false,
            EmailAddress = "test2@test.com",
        };

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.UpdateUserBasicDataAsync(user.Id, updateUserDto);
        });
    }

    [TestMethod]
    public async Task UpdateUserPassword_ShouldUpdateUserPassword()
    {
        var hasherMock = new Mock<IHasher>();
        hasherMock.Setup(hasher => hasher.GetHash(It.IsAny<string>()))
            .Returns<string>(pwd => (pwd, pwd));

        var service = new UsersService(
            hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        var user = await service.CreateUserAsync(GetDefaultCreateUserDto());
        var sourcePasswordHash = user.PasswordHash;
        var sourcePasswordSalt = user.PasswordSalt;
        var updatedUser = await service.UpdateUserPasswordAsync(user.Id, "54321");

        Assert.AreNotEqual(sourcePasswordHash, updatedUser.PasswordHash);
        Assert.AreNotEqual(sourcePasswordSalt, updatedUser.PasswordSalt);
    }

    [TestMethod]
    public async Task UpdateUserWithEmailInuse_ShouldThrowCoreLogicException()
    {
        var service = new UsersService(
            _hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            CreateInMemoryContext()
        );

        var user = await service.CreateUserAsync(GetDefaultCreateUserDto());
        var userDto2 = GetDefaultCreateUserDto();
        userDto2.EmailAddress = "test2@test.com";
        var user2 = await service.CreateUserAsync(userDto2);

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            var updateDto = GetDefaultCreateUserDto();
            updateDto.EmailAddress = user2.EmailAddress;
            await service.UpdateUserBasicDataAsync(user.Id, updateDto);
        });
    }

    [TestMethod]
    public async Task UpdateUserWithInvalidPassword_ShouldThrowCoreLogicException()
    {
        var context = CreateInMemoryContext();

        const int userId = 1;
        context.Users.Add(new()
        {
            Id = userId,
            FirstName = "FirstName",
            LastName = "LastName",
            Active = true,
            EmailAddress = "test@test.com",
            PasswordHash = "12345hashed",
            PasswordSalt = "salted"
        });
        await context.SaveChangesAsync();

        var service = new UsersService(
            _hasherMock.Object,
            _alwaysIncorrectPasswordPolicy.Object,
            context
        );

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.UpdateUserPasswordAsync(userId, "12345");
        });
    }

    [TestMethod]
    public async Task DeleteUser_ShouldDeleteUser()
    {
        var context = CreateInMemoryContext();
        const int userId = 1;
        context.Users.Add(new()
        {
            Id = userId,
            FirstName = "FirstName",
            LastName = "LastName",
            Active = true,
            EmailAddress = "test@test.com",
            PasswordHash = "12345hashed",
            PasswordSalt = "salted"
        });
        await context.SaveChangesAsync();

        var service = new UsersService(
            _hasherMock.Object,
            _alwaysCorrectPasswordPolicy.Object,
            context
        );

        await service.RemoveUserAsync(userId);

        var removedUser = await context.Users.FindAsync(userId);
        Assert.IsNull(removedUser);
    }

    private static UsersContext CreateInMemoryContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<UsersContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new UsersContext(optionsBuilder.Options);
    }

    private static CreateUserDto GetDefaultCreateUserDto() => new()
    {
        FirstName = "FirstName",
        LastName = "LastName",
        Active = true,
        EmailAddress = "test@test.com",
        Password = "12345"
    };
}