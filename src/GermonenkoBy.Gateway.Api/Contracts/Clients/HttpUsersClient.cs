using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Utils;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Models.Users;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public class HttpUsersClient : IUsersClient
{
    private readonly HttpClientFacade _httpClient;

    public HttpUsersClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<PagedSet<User>> GetUsersAsync(UsersFilterDto usersFilter)
    {
        var query = usersFilter.ToDictionary();
        var response = await _httpClient.GetAsync<ContentListResponse<User>>("api/users", query);
        return new PagedSet<User>
        {
            Total = response.Total,
            Count = response.Count,
            Offset = response.Offset,
            Data = response.Data ?? new List<User>()
        };
    }
}