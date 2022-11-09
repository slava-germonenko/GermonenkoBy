using GermonenkoBy.Common.Domain;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface ICategoriesClient
{
    public Task<PagedSet<Category>> GetCategoriesAsync(CategoriesFilterDto categoriesFilter);

    public Task<Category> GetCategoryAsync(int categoryId);

    public Task<Category> CreateCategoryAsync(SaveCategoryDto categoryDto);

    public Task<Category> UpdateCategoryAsync(int categoryId, SaveCategoryDto categoryDto);

    public Task DeleteCategoryAsync(int categoryId);
}