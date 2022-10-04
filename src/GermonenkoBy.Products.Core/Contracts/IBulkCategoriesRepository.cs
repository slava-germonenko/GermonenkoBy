namespace GermonenkoBy.Products.Core.Contracts;

public interface IBulkCategoriesRepository
{
    public Task ReassignCategoryAsync(int categoryId, int? newCategoryId = null);
}