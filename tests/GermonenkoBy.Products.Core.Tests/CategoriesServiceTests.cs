using Microsoft.EntityFrameworkCore;
using Moq;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Products.Core.Contracts;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core.Tests;

[TestClass]
public class CategoriesServiceTests : BaseProductModuleTest
{
    private const int DefaultTestCategoryId = 1;

    private readonly Mock<IBulkCategoriesRepository> _bulkCategoriesRepositoryMock = new();

    private IBulkCategoriesRepository BulkRepository => _bulkCategoriesRepositoryMock.Object;

    public CategoriesServiceTests()
    {
        _bulkCategoriesRepositoryMock.Setup(
                repo => repo.ReassignCategoryAsync(
                    It.IsAny<int>(),
                    It.IsAny<int?>()
                )
            )
            .Returns(Task.CompletedTask);
    }

    [TestMethod]
    public async Task GetCategory_ShouldReturnCategory()
    {
        var context = CreateInMemoryContext();
        context.Categories.Add(new Category { Id = DefaultTestCategoryId, Name = "test"});
        await context.SaveChangesAsync();

        var service = new CategoriesService(context, BulkRepository);

        var category = await service.GetCategoryAsync(DefaultTestCategoryId);
        Assert.AreEqual("test", category.Name);
    }

    [TestMethod]
    public async Task GetNotExistingCategory_ShouldThrowNotFoundException()
    {
        var service = new CategoriesService(CreateInMemoryContext(), BulkRepository);

        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await service.GetCategoryAsync(DefaultTestCategoryId);
        });
    }

    [TestMethod]
    public async Task CreateCategory_ShouldCreateCategory()
    {
        var context = CreateInMemoryContext();
        var service = new CategoriesService(context, BulkRepository);

        await service.CreateCategoryAsync(new()
        {
            Name = "Test",
        });

        var category = context.Categories.First();
        Assert.AreEqual("Test", category.Name);
    }

    [TestMethod]
    public async Task CreateDuplicateCategory_ShouldThrowCoreLogicException()
    {
        var service = new CategoriesService(CreateInMemoryContext(), BulkRepository);

        await service.CreateCategoryAsync(new()
        {
            Name = "Test",
        });

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.CreateCategoryAsync(new()
            {
                Name = "Test",
            });
        });
    }

    [TestMethod]
    public async Task UpdateCategory_ShouldUpdateCategory()
    {
        var service = new CategoriesService(CreateInMemoryContext(), BulkRepository);
        var category = await service.CreateCategoryAsync(new()
        {
            Name = "Test",
        });

        await service.UpdateCategoryAsync(category.Id, new() { Name = "Test2" });
        var updatedCategory = await service.GetCategoryAsync(category.Id);

        Assert.AreEqual(category.Id, updatedCategory.Id);
        Assert.AreEqual("Test2", updatedCategory.Name);
    }

    [TestMethod]
    public async Task UpdateCategoryWithDuplicateName_ShouldThrowCoreLogicException()
    {
        var service = new CategoriesService(CreateInMemoryContext(), BulkRepository);
        await service.CreateCategoryAsync(new() { Name = "Test" });
        var category = await service.CreateCategoryAsync(new() { Name = "Test2" });

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.UpdateCategoryAsync(category.Id, new() { Name = "Test"});
        });
    }

    [TestMethod]
    public async Task UpdateMissingCategory_ShouldThrowNotFoundException()
    {
        var service = new CategoriesService(CreateInMemoryContext(), BulkRepository);

        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await service.UpdateCategoryAsync(5, new SaveCategoryDto { Name = "123" });
        });
    }

    [TestMethod]
    public async Task RemoveCategory_ShouldRemoveCategoryAndUpdateProducts()
    {
        var context = CreateInMemoryContext();
        var service = new CategoriesService(context, BulkRepository);

        var category = new Category
        {
            Name = "Test",
        };
        var product = new Product
        {
            Name = "Test",
            InternationalName = "Test",
            ItemNumber = "AA-0000",
            Category = category,
        };

        context.Add(product);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        await service.RemoveCategoryAsync(category.Id);

        var removedCategory = context.Categories.FirstOrDefault();
        var updatedProduct = context.Products.Include(p => p.Category).First();

        Assert.IsNull(removedCategory);
        Assert.IsNull(updatedProduct.Category);
    }
}