using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Products.Core.Contracts;
using GermonenkoBy.Products.Core.Contracts.Repositories;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core;

public class CategoriesService
{
    private readonly ProductsContext _context;

    private readonly IBulkCategoriesRepository _bulkCategoriesRepository;

    public CategoriesService(
        ProductsContext context,
        IBulkCategoriesRepository bulkCategoriesRepository
    )
    {
        _context = context;
        _bulkCategoriesRepository = bulkCategoriesRepository;
    }

    public async Task<Category> GetCategoryAsync(int categoryId)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category is null)
        {
            throw new NotFoundException($"Категория товаров с индентификатором \"{categoryId}\" не найдена.");
        }

        return category;
    }

    public async Task<Category> CreateCategoryAsync(SaveCategoryDto categoryDto)
    {
        var categoryNameIsInUse = await _context.Categories.AnyAsync(c => c.Name == categoryDto.Name);
        if (categoryNameIsInUse)
        {
            throw new CoreLogicException($"Название категории \"{categoryDto.Name}\" уже используется.");
        }

        var category = new Category { Name = categoryDto.Name };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task<Category> UpdateCategoryAsync(int categoryId, SaveCategoryDto categoryDto)
    {
        var categoryNameIsInUse = await _context.Categories.AnyAsync(
            c => c.Name == categoryDto.Name && c.Id != categoryId
        );
        if (categoryNameIsInUse)
        {
            throw new CoreLogicException($"Название категории \"{categoryDto.Name}\" уже существует.");
        }

        var category = await _context.Categories.FindAsync(categoryId);
        if (category is null)
        {
            throw new NotFoundException($"Категория товаров с индентификатором \"{categoryId}\" не найдена.");
        }

        category.Name = categoryDto.Name;

        _context.Categories.Update(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task RemoveCategoryAsync(int categoryId, int? assignTo = null)
    {
        var category = await _context.Categories.FindAsync(categoryId);
        if (category is null)
        {
            return;
        }

        if (assignTo is not null)
        {
            var assignToCategoryExists = await _context.Categories.AnyAsync(c => c.Id == assignTo.Value);
            if (!assignToCategoryExists)
            {
                throw new NotFoundException($"Категория товаров с индентификатором \"{assignTo.Value}\" не найдена.");
            }
        }

        await _bulkCategoriesRepository.ReassignCategoryAsync(categoryId, categoryId);

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}