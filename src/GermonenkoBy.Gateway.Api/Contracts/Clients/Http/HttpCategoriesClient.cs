using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Utils;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Extensions;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

public class HttpCategoriesClient : ICategoriesClient
{
    private readonly HttpClientFacade _httpClient;

    private const string BaseRoute = "api/categories";

    private const string SerializationErrorMessage = "Произошла ошибка при попытке сериализовать категорию.";

    public HttpCategoriesClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<PagedSet<Category>> GetCategoriesAsync(CategoriesFilterDto categoriesFilter)
    {
        var categories = await _httpClient.GetAsync<ContentListResponse<Category>>(
            BaseRoute,
            categoriesFilter.ToDictionary()
        );
        return categories.ToPagedSet();
    }

    public async Task<Category> GetCategoryAsync(int categoryId)
    {
        var url = GetCategoryRoute(categoryId);
        var response = await _httpClient.GetAsync<ContentResponse<Category>>(url);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public async Task<Category> CreateCategoryAsync(SaveCategoryDto categoryDto)
    {
        var response = await _httpClient.PostAsync<ContentResponse<Category>>(BaseRoute, body: categoryDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public async Task<Category> UpdateCategoryAsync(int categoryId, SaveCategoryDto categoryDto)
    {
        var url = GetCategoryRoute(categoryId);
        var response = await _httpClient.PatchAsync<ContentResponse<Category>>(url, body: categoryDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public Task DeleteCategoryAsync(int categoryId)
        => _httpClient.DeleteAsync(
            GetCategoryRoute(categoryId)
        );

    private static string GetCategoryRoute(int categoryId) => $"{BaseRoute}/{categoryId}";
}