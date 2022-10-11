namespace GermonenkoBy.Products.Core.Tests;

[TestClass]
public class MaterialsSearchServiceTests : BaseProductModuleTest
{
    [TestMethod]
    public async Task SearchMaterials_ShouldReturnCorrectResults()
    {
        var context = CreateInMemoryContext();
        context.Materials.Add(new()
        {
            Name = "1",
        });
        context.Materials.Add(new()
        {
            Name = "12",
        });
        context.Materials.Add(new()
        {
            Name = "123",
        });
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var service = new MaterialsSearchService(context);
        var materials = await service.SearchMaterialsAsync(new()
        {
            Count = 10,
            Name = "1",
        });

        Assert.AreEqual(1, materials.Data.Count);
        Assert.AreEqual(1, materials.Total);
        Assert.AreEqual("1", materials.Data.First().Name);

        materials = await service.SearchMaterialsAsync(new()
        {
            Count = 10,
            Search = "12",
        });
        Assert.AreEqual(2, materials.Data.Count);
        Assert.AreEqual(2, materials.Total);

        materials = await service.SearchMaterialsAsync(new()
        {
            Count = 10,
            OrderBy = "name",
            OrderDirection = "DESC"
        });

        Assert.AreEqual(3, materials.Data.Count);
        Assert.AreEqual("123", materials.Data.First().Name);

        materials = await service.SearchMaterialsAsync(new()
        {
            Count = 10,
            OrderBy = "name",
            OrderDirection = "ASC"
        });

        Assert.AreEqual(3, materials.Data.Count);
        Assert.AreEqual("1", materials.Data.First().Name);
    }
}