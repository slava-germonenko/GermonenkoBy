using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GermonenkoBy.Products.Infrastructure.Repositories;

public class BulkCategoriesRepository : IBulkCategoriesRepository
{
    private readonly ProductsContext _context;

    public BulkCategoriesRepository(ProductsContext context)
    {
        _context = context;
    }

    public async Task ReassignCategoryAsync(int categoryId, int? newCategoryId = null)
    {
        await _context.Database.ExecuteSqlRawAsync($@"
            UPDATE [dbo].[Products] SET [CategoryId] = {newCategoryId}
            WHERE [CategoryId] = {categoryId}
        ");
    }
}