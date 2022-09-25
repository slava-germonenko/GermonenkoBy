using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Users.Core.Models;

namespace GermonenkoBy.Users.Core.Tests;

[TestClass]
public class UsersServiceSearchTests
{
    [TestMethod]
    public async Task SearchUsers_ShouldReturnCorrectUsers()
    {
        var context = CreateInMemoryContext();
        var service = new UsersSearchService(context);
        context.Users.Add(new User
        {
            Id = 1,
            FirstName = "FirstName",
            LastName = "LastName",
            EmailAddress = "EmailAddress@test.com",
        });
        context.Users.Add(new User
        {
            Id = 2,
            FirstName = "FirstName2",
            LastName = "LastName2",
            EmailAddress = "EmailAddress2@test.com",
        });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        const int testCount = 1;
        var countResult = await service.SearchUsersListAsync(new()
        {
            Count = testCount,
        });
        Assert.AreEqual(testCount, countResult.Data.Count);

        var firstNameResult = await service.SearchUsersListAsync(new()
        {
            FirstName = "FirstName",
            Count = 10,
        });
        Assert.AreEqual(1, firstNameResult.Data.Count);
        Assert.AreEqual("FirstName", firstNameResult.Data.First().FirstName);

        var lastNameResult = await service.SearchUsersListAsync(new()
        {
            LastName = "LastName",
            Count = 10,
        });
        Assert.AreEqual(1, lastNameResult.Data.Count);
        Assert.AreEqual("LastName", lastNameResult.Data.First().LastName);

        var emailAddressResult = await service.SearchUsersListAsync(new()
        {
            EmailAddress = "EmailAddress@test.com",
            Count = 10,
        });
        Assert.AreEqual(1, emailAddressResult.Data.Count);
        Assert.AreEqual("EmailAddress@test.com", emailAddressResult.Data.First().EmailAddress);

        var searchResult = await service.SearchUsersListAsync(new()
        {
            Search = "Name",
            Count = 10,
        });
        Assert.AreEqual(2, searchResult.Data.Count);
    }

    [DataTestMethod]
    [DataRow("ASC", "firstName")]
    [DataRow("DESC", "firstName")]
    [DataRow("ASC", "lastName")]
    [DataRow("DESC", "lastName")]
    [DataRow("ASC", "emailAddress")]
    [DataRow("DESC", "emailAddress")]
    public async Task OrderUsers_ShouldOrderCorrectly(string orderDirection, string orderField)
    {
        var context = CreateInMemoryContext();
        var service = new UsersSearchService(context);
        context.Add(new User
        {
            Id = 1,
            FirstName = "1",
            LastName = "1",
            EmailAddress = "1@test.com",
        });
        context.Add(new User
        {
            Id = 2,
            FirstName = "2",
            LastName = "2",
            EmailAddress = "2@test.com",
        });
        context.Add(new User
        {
            Id = 3,
            FirstName = "3",
            LastName = "3",
            EmailAddress = "3@test.com",
        });
        await context.SaveChangesAsync();

        var usersSet = await service.SearchUsersListAsync(new ()
        {
            Count = 10,
            OrderBy = orderField,
            OrderDirection = orderDirection
        });

        if (orderDirection == "ASC")
        {
            Assert.AreEqual("1", usersSet.Data.First().FirstName);
        }
        else
        {
            Assert.AreEqual("3", usersSet.Data.First().FirstName);
        }
    }

    private UsersContext CreateInMemoryContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<UsersContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new UsersContext(optionsBuilder.Options);
    }
}