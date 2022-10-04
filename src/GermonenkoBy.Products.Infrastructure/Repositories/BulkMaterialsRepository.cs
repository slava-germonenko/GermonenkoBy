using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Contracts;
using GermonenkoBy.Products.Core.Contracts.Repositories;

namespace GermonenkoBy.Products.Infrastructure.Contracts;

public class BulkMaterialsRepository : IBulkMaterialsRepository
{
    private readonly ProductsContext _context;

    public BulkMaterialsRepository(ProductsContext context)
    {
        _context = context;
    }

    public async Task ReassignMaterialAsync(int materialId, int? newMaterialId)
    {
        var newMaterialIdStr = newMaterialId is null ? "NULL" : newMaterialId.Value.ToString();
        await _context.Database.ExecuteSqlRawAsync($@"
            UPDATE [dbo].[Products] SET [MaterialId] = {newMaterialIdStr}
            WHERE [MaterialId] = {materialId}
        ");
    }
}