using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Extensions;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core;

public class ProductsService
{
    private readonly ProductsContext _context;

    private readonly MaterialsService _materialsService;

    private readonly CategoriesService _categoriesService;

    public ProductsService(
        ProductsContext context,
        MaterialsService materialsService,
        CategoriesService categoriesService
    )
    {
        _context = context;
        _materialsService = materialsService;
        _categoriesService = categoriesService;
    }

    public async Task<Product> GetProductAsync(int productId)
    {
        var product = await _context.Products.AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Material)
            .Include(p => p.Prices)
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
        {
            throw new NotFoundException($"Товар и индентификатором \"{productId}\" не найден.");
        }

        return product;
    }

    public async Task<Product> CreateProductAsync(CreateProductDto productDto)
    {
        CoreValidationHelper.EnsureEntityIsValid(productDto);

        var productItemNumberIsInUse = await _context.Products.AnyAsync(p => p.ItemNumber == productDto.ItemNumber);
        if (productItemNumberIsInUse)
        {
            throw new CoreLogicException($"Артикул товара \"{productDto.ItemNumber}\" уже используется.");
        }

        var product = new Product();
        product.CopyDetailsFrom(productDto);

        if (productDto.CategoryId is not null)
        {
            product.Category = await _categoriesService.GetCategoryAsync(productDto.CategoryId.Value);
        }

        if (productDto.MaterialId is not null)
        {
            product.Material = await _materialsService.GetMaterialAsync(productDto.MaterialId.Value);
        }

        if (productDto.ProductPrices is not null)
        {
            product.Prices = productDto.ProductPrices
                .DistinctBy(dto => dto.PriceType)
                .Select(dto => new ProductPrice
                {
                    PriceType = dto.PriceType,
                    Price = dto.Price
                })
                .ToList();
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateProductDetails(int productId, ModifyProductDto productDto)
    {
        CoreValidationHelper.EnsureEntityIsValid(productDto);

        var productItemNumberIsInUse = await _context.Products.AnyAsync(
            p => p.ItemNumber == productDto.ItemNumber && p.Id != productId
        );
        if (productItemNumberIsInUse)
        {
            throw new CoreLogicException($"Артикул товара \"{productDto.ItemNumber}\" уже используется.");
        }

        var product = await GetProductAsync(productId);

        if (productDto.CategoryId is not null)
        {
            product.Category = await _categoriesService.GetCategoryAsync(productDto.CategoryId.Value);
        }

        if (productDto.MaterialId is not null)
        {
            product.Material = await _materialsService.GetMaterialAsync(productDto.MaterialId.Value);
        }

        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> SetProductPricesAsync(int productId, ICollection<AddProductPriceDto> productPrices)
    {
        var product = await GetProductAsync(productId);
        product.Prices = productPrices
            .DistinctBy(dto => dto.PriceType)
            .Select(dto => new ProductPrice
            {
                PriceType = dto.PriceType,
                Price = dto.Price
            })
            .ToList();

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task RemoveProductAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product is not null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}