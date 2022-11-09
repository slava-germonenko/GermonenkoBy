using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Gateway.Api.Extensions;

public static class PagedSetExtensions
{
    public static PagedSet<TData> ToPagedSet<TData>(this ContentListResponse<TData> listResponse)
        => new()
        {
            Count = listResponse.Count,
            Offset = listResponse.Offset,
            Total = listResponse.Total,
            Data = listResponse.Data ?? new List<TData>()
        };
}