using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
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

    public async Task<User?> GetUserAsync(int userId)
    {
        try
        {
            var url = $"api/users/{userId}";
            var response = await _httpClient.GetAsync<ContentResponse<User>>(url);
            return response.Data;
        }
        catch (NotFoundException)
        {
            return null;
        }
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