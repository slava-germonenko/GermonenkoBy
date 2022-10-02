using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core;

public class CategoriesSearchService
{
    private readonly ProductsContext _context;

    public CategoriesSearchService(ProductsContext context)
    {
        _context = context;
    }

    public async Task<PagedSet<Category>> SearchCategoriesAsync(CategoriesFilterDto filter)
    {
        var query = _context.Categories.AsNoTracking();

        if (filter.Name is not null)
        {
            query = query.Where(category => category.Name == filter.Name);
        }

        if (filter.Search is not null)
        {
            query = query.Where(category => category.Name.Contains(filter.Search));
        }

        var descendingOrder = filter.OrderBy is null
                              || filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        Expression<Func<Category, object>> orderKeySelector = filter.OrderBy?.ToLower() switch
        {
            "updateddate" => category => category.UpdatedDate,
            "name" => category => category.Name,
            _ => category => category.CreatedDate
        };

        query = descendingOrder
            ? query.OrderByDescending(orderKeySelector)
            : query.OrderBy(orderKeySelector);

        return await query.ToPagedSetAsync(filter);
    }
}