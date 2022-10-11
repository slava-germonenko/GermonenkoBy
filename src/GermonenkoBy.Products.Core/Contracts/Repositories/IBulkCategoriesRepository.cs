namespace GermonenkoBy.Products.Core.Contracts.Repositories;

public interface IBulkCategoriesRepository
{
    public Task ReassignCategoryAsync(int categoryId, int? newCategoryId = null);
}