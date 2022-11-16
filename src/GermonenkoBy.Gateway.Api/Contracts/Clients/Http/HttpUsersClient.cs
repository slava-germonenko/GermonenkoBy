using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Utils;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Extensions;
using GermonenkoBy.Gateway.Api.Models.Users;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

public class HttpUsersClient : IUsersClient
{
    private readonly HttpClientFacade _httpClient;

    private const string UserSerializationErrorMessage = "Произошла ошибка при попытке серализовать объект пользователя.";

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
        var response = await _httpClient.GetAsync<ContentListResponse<User>>(
            "api/users",
            usersFilter.ToDictionary()
        );
        return response.ToPagedSet();
    }

    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        var response = await _httpClient.PostAsync<ContentResponse<User>>("api/users", body: userDto);
        return response.Data ?? throw new Exception(UserSerializationErrorMessage);
    }

    public async Task<User> UpdateUserAsync(int userId, ModifyUserDto userDto)
    {
        var response = await _httpClient.PatchAsync<ContentResponse<User>>($"api/users/{userDto}", body: userDto);
        return response.Data ?? throw new Exception(UserSerializationErrorMessage);
    }

    public Task SetUserPasswordAsync(int userId, string password)
        => _httpClient.PatchAsync($"api/users/{userId}/password", body: new { password });

    public Task DeleteUserAsync(int userId) => _httpClient.DeleteAsync($"api/user/{userId}");
}