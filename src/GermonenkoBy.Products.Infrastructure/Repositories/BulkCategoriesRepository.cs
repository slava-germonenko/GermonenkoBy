using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Contracts;
using GermonenkoBy.Products.Core.Contracts.Repositories;

namespace GermonenkoBy.Products.Infrastructure.Contracts;

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