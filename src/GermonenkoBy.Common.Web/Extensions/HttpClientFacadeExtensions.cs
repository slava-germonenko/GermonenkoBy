using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Common.Web.Extensions;

public static class HttpClientFacadeExtensions
{
    public static async Task<TData?> GetUnwrappedAsync<TData>(
        this HttpClientFacade httpClient,
        string route,
        IDictionary<string, string?>? queryParams = null
    )
    {
        var response = await httpClient.GetAsync<ContentResponse<TData>>(route, queryParams);
        return response.Data;
    }

    public static async Task<ICollection<TItem>> GetUnwrappedListAsync<TItem>(
        this HttpClientFacade httpClient,
        string route,
        IDictionary<string, string?>? queryParams = null
    )
    {
        var response = await httpClient.GetAsync<ContentListResponse<TItem>>(route, queryParams);
        return response.Data ?? new List<TItem>();
    }
}