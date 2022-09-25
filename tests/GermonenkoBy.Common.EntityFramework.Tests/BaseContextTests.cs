namespace GermonenkoBy.Common.EntityFramework.Tests;

[TestClass]
public class BaseContextTests
{
    private const int DefaultTestModelId = 1;

    [TestMethod, TestCategory("EF")]
    public async Task CreatedModel_ShouldSetCreatedDateOnly()
    {
        var context = CreateTestContext();
        var entity = await context.TestModels.FindAsync(DefaultTestModelId);

        Assert.IsNull(entity!.UpdatedDate);
        Assert.AreNotEqual(default, entity.CreatedDate);
    }

    [DataTestMethod, TestCategory("EF")]
    [DataRow(true, false)]
    [DataRow(true, true)]
    [DataRow(false, false)]
    [DataRow(false, true)]
    public void UpdateModel_ShouldSetUpdatedDateOnly(bool acceptAllChangesOnSuccess, bool useAsync)
    {
        var context = CreateTestContext();
        var entity = context.TestModels.Find(DefaultTestModelId)!;
        entity.EmailAddress = "test2@test.com";

        var initialCreatedDate = entity.CreatedDate;
        context.TestModels.Update(entity);
        CallSaveChanges(context, acceptAllChangesOnSuccess, useAsync);

        // Created Date should not be changed.
        Assert.AreEqual(initialCreatedDate, entity.CreatedDate);
        Assert.IsNotNull(entity.UpdatedDate);
        Assert.AreNotEqual(default(DateTime), entity.UpdatedDate);
    }

    [DataTestMethod, TestCategory("EF")]
    [DataRow(true, false)]
    [DataRow(true, true)]
    [DataRow(false, false)]
    [DataRow(false, true)]
    public void CreateModel_ShouldNotSetCustomCreatedDateAndUpdatedDate(bool acceptAllChangesOnSuccess, bool useAsync)
    {
        var context = new TestContext();
        var customCreatedDate = DateTime.UtcNow.AddDays(-1);

        context.TestModels.Add(new ()
        {
            Id = DefaultTestModelId,
            CreatedDate = customCreatedDate,
            UpdatedDate = customCreatedDate,
        });
        CallSaveChanges(context, acceptAllChangesOnSuccess, useAsync);

        var testModel = context.TestModels.Find(DefaultTestModelId)!;
        Assert.AreNotEqual(customCreatedDate, testModel.CreatedDate);
        Assert.AreNotEqual(customCreatedDate, testModel.UpdatedDate);
    }

    [DataTestMethod, TestCategory("EF")]
    [DataRow(true, false)]
    [DataRow(true, true)]
    [DataRow(false, false)]
    [DataRow(false, true)]
    public void UpdateModel_ShouldNotSetCustomCreatedDateAndUpdatedDate(bool acceptAllChangesOnSuccess, bool useAsync)
    {
        var context = CreateTestContext();

        var customUpdatedDate = DateTime.UtcNow.AddDays(-1);
        var testModel = context.TestModels.Find(DefaultTestModelId)!;

        testModel.CreatedDate = customUpdatedDate;
        testModel.UpdatedDate = customUpdatedDate;

        context.Update(testModel);
        CallSaveChanges(context, acceptAllChangesOnSuccess, useAsync);

        testModel = context.TestModels.Find(DefaultTestModelId)!;
        Assert.AreNotEqual(customUpdatedDate, testModel.CreatedDate);
        Assert.AreNotEqual(customUpdatedDate, testModel.UpdatedDate);
    }

    private TestContext CreateTestContext()
    {
        var context = new TestContext();
        context.TestModels.Add(new ()
        {
            Id = DefaultTestModelId
        });
        context.SaveChanges();
        return context;
    }

    private static Task CallSaveChanges(TestContext context, bool acceptAllChangesOnSuccess, bool useAsync)
    {
        if (acceptAllChangesOnSuccess && !useAsync)
        {
            context.SaveChanges(acceptAllChangesOnSuccess);
            return Task.CompletedTask;
        }

        if (!acceptAllChangesOnSuccess && !useAsync)
        {
            context.SaveChanges();
            return Task.CompletedTask;
        }

        if (!acceptAllChangesOnSuccess && useAsync)
        {
            return context.SaveChangesAsync();
        }

        if (acceptAllChangesOnSuccess && useAsync)
        {
            return context.SaveChangesAsync(acceptAllChangesOnSuccess);
        }


        return Task.CompletedTask;
    }
}