using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core;

public class MaterialsSearchService
{
    private readonly ProductsContext _context;

    public MaterialsSearchService(ProductsContext context)
    {
        _context = context;
    }

    public async Task<PagedSet<Material>> SearchMaterialsAsync(MaterialsFilterDto filter)
    {
        var query = _context.Materials.AsNoTracking();

        if (filter.Name is not null)
        {
            query = query.Where(material => material.Name == filter.Name);
        }

        if (filter.Search is not null)
        {
            query = query.Where(material => material.Name.Contains(filter.Search));
        }

        var descendingOrder = filter.OrderBy is null
                              || filter.OrderDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        Expression<Func<Material, object>> orderKeySelector = filter.OrderBy?.ToLower() switch
        {
            "updateddate" => material => material.UpdatedDate,
            "name" => material => material.Name,
            _ => material => material.CreatedDate
        };

        query = descendingOrder
            ? query.OrderByDescending(orderKeySelector)
            : query.OrderBy(orderKeySelector);

        return await query.ToPagedSetAsync(filter);
    }
}