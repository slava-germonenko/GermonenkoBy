using Microsoft.EntityFrameworkCore;
using Moq;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Products.Core.Contracts;
using GermonenkoBy.Products.Core.Contracts.Repositories;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core.Tests;

[TestClass]
public class MaterialsServiceTests : BaseProductModuleTest
{
    private readonly Mock<IBulkMaterialsRepository> _bulkMaterialsRepositoryMock = new();

    private IBulkMaterialsRepository BulkRepository => _bulkMaterialsRepositoryMock.Object;

    public MaterialsServiceTests()
    {
        _bulkMaterialsRepositoryMock.Setup(repo => repo.ReassignMaterialAsync(
                    It.IsAny<int>(),
                    It.IsAny<int?>()
                )
            )
            .Returns(Task.CompletedTask);
    }

    [TestMethod]
    public async Task GetMaterial_ShouldReturnMaterials()
    {
        var context = CreateInMemoryContext();
        var material = new Material
        {
            Id = 1,
            Name = "Test",
        };
        context.Materials.Add(material);
        await context.SaveChangesAsync();

        var service = new MaterialsService(context, BulkRepository);

        var returnedMaterial = await service.GetMaterialAsync(material.Id);
        Assert.IsNotNull(returnedMaterial);
        Assert.AreEqual("Test", returnedMaterial.Name);
    }

    [TestMethod]
    public async Task GetNotExistingMaterial_ShouldThrowNotFoundException()
    {
        var service = new MaterialsService(CreateInMemoryContext(), BulkRepository);

        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await service.GetMaterialAsync(5);
        });
    }

    [TestMethod]
    public async Task CreateMaterial_ShouldCreateMaterial()
    {
        var service = new MaterialsService(CreateInMemoryContext(), BulkRepository);
        var material = service.CreateMaterialAsync(new() { Name = "Test" });
        var createdMaterial = await service.GetMaterialAsync(material.Id);

        Assert.AreEqual("Test", createdMaterial.Name);
    }

    [TestMethod]
    public async Task CreateDuplicateMaterial_ShouldThrowCoreLogicException()
    {
        var service = new MaterialsService(CreateInMemoryContext(), BulkRepository);
        await service.CreateMaterialAsync(new() { Name = "Test" });

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.CreateMaterialAsync(new() { Name = "Test" });
        });
    }

    [TestMethod]
    public async Task UpdateMaterial_ShouldUpdateMaterial()
    {
        var service = new MaterialsService(CreateInMemoryContext(), BulkRepository);
        var material = await service.CreateMaterialAsync(new()
        {
             Name= "Test",
        });

        await service.UpdateMaterialAsync(material.Id, new() { Name = "Test2" });
        var updatedMaterial = await service.GetMaterialAsync(material.Id);

        Assert.AreEqual(material.Id, updatedMaterial.Id);
        Assert.AreEqual("Test2", updatedMaterial.Name);
    }

    [TestMethod]
    public async Task UpdateMaterialWithDuplicateName_ShouldThrowCoreLogicException()
    {
        var service = new MaterialsService(CreateInMemoryContext(), BulkRepository);
        await service.CreateMaterialAsync(new() { Name = "Test" });
        var material = await service.CreateMaterialAsync(new() { Name = "Test2" });

        await Assert.ThrowsExceptionAsync<CoreLogicException>(async () =>
        {
            await service.UpdateMaterialAsync(material.Id, new() { Name = "Test"});
        });
    }

    [TestMethod]
    public async Task UpdateMissingMaterial_ShouldThrowNotFoundException()
    {
        var service = new MaterialsService(CreateInMemoryContext(), BulkRepository);

        await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
        {
            await service.UpdateMaterialAsync(5, new SaveMaterialDto { Name = "123" });
        });
    }

    [TestMethod]
    public async Task RemoveMaterial_ShouldRemoveCategoryAndUpdateProducts()
    {
        var context = CreateInMemoryContext();
        var service = new MaterialsService(context, BulkRepository);

        var material = new Material
        {
            Name = "Test",
        };
        var product = new Product
        {
            Name = "Test",
            InternationalName = "Test",
            ItemNumber = "AA-0000",
            Material = material,
        };

        context.Add(product);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        await service.RemoveMaterialAsync(material.Id);

        var removedMaterial = context.Materials.FirstOrDefault();
        var updatedProduct = context.Products.Include(p => p.Category).First();

        Assert.IsNull(removedMaterial);
        Assert.IsNull(updatedProduct.Category);
    }
}