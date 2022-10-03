using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

using Microsoft.EntityFrameworkCore;

namespace GermonenkoBy.Products.Core;

public class ProductsSearchService
{
    private readonly ProductsContext _context;

    public ProductsSearchService(ProductsContext context)
    {
        _context = context;
    }

    public async Task<PagedSet<Product>> SearchProductsAsync(ProductsFilterDto filter)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Material)
            .Include(p => p.Prices)
            .AsNoTracking();

        if (filter.Name is not null)
        {
            query = query.Where(p => p.Name == filter.Name);
        }

        if (filter.ItemNumber is not null)
        {
            query = query.Where(p => p.ItemNumber == filter.ItemNumber);
        }

        if (filter.InternationalName is not null)
        {
            query = query.Where(p => p.InternationalName == filter.InternationalName);
        }

        if (filter.Active is not null)
        {
            query = query.Where(p => p.Active == filter.Active);
        }

        if (filter.MaterialId is not null)
        {
            query = query.Where(p => p.Material.Id == filter.MaterialId);
        }

        if (filter.CategoryId is not null)
        {
            query = query.Where(p => p.Category.Id == filter.CategoryId);
        }

        var search = filter.Search;
        if (search is not null)
        {
            query = query.Where(
                p => p.Name.Contains(search)
                     || p.ItemNumber.Contains(search)
                     || p.InternationalName.Contains(search)
            );
        }

        var descendingOrder = filter.OrderBy is null
                              || filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);



        return await query.ToPagedSetAsync(filter);
    }
}