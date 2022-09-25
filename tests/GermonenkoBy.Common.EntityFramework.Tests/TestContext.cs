using Microsoft.EntityFrameworkCore;

namespace GermonenkoBy.Common.EntityFramework.Tests;

public class TestContext : BaseContext
{
    public DbSet<TestModel> TestModels => Set<TestModel>();

    public TestContext() : base(CreateTestOptions())
    {
    }

    private static DbContextOptions<TestContext> CreateTestOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        return optionsBuilder.Options;
    }
}