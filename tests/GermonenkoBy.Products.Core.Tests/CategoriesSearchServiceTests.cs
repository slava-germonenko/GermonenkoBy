namespace GermonenkoBy.Products.Core.Tests;

[TestClass]
public class CategoriesSearchServiceTests : BaseProductModuleTest
{
    [TestMethod]

    public async Task SearchCategories_ShouldReturnCorrectResults()
    {
        var context = CreateInMemoryContext();
        context.Categories.Add(new()
        {
            Name = "1",
        });
        context.Categories.Add(new()
        {
            Name = "12",
        });
        context.Categories.Add(new()
        {
            Name = "123",
        });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var service = new CategoriesSearchService(context);
        var categories = await service.SearchCategoriesAsync(new()
        {
            Count = 10,
            Name = "1",
        });

        Assert.AreEqual(1, categories.Data.Count);
        Assert.AreEqual(1, categories.Total);
        Assert.AreEqual("1", categories.Data.First().Name);

        categories = await service.SearchCategoriesAsync(new()
        {
            Count = 10,
            Search = "12",
        });
        Assert.AreEqual(2, categories.Data.Count);
        Assert.AreEqual(2, categories.Total);

        categories = await service.SearchCategoriesAsync(new()
        {
            Count = 10,
            OrderBy = "name",
            OrderDirection = "DESC"
        });

        Assert.AreEqual(3, categories.Data.Count);
        Assert.AreEqual("123", categories.Data.First().Name);

        categories = await service.SearchCategoriesAsync(new()
        {
            Count = 10,
            OrderBy = "name",
            OrderDirection = "ASC"
        });

        Assert.AreEqual(3, categories.Data.Count);
        Assert.AreEqual("1", categories.Data.First().Name);
    }
}