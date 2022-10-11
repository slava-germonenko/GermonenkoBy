using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Common.EntityFramework.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedSet<TModel>> ToPagedSetAsync<TModel>(
        this IQueryable<TModel> query,
        Paging paging
    )
    {
        var data = await query.Skip(paging.Offset)
            .Take(paging.Count)
            .ToListAsync();

        var total = await query.CountAsync();

        return new ()
        {
            Count = paging.Count,
            Offset = paging.Offset,
            Total = total,
            Data = data,
        };
    }
}