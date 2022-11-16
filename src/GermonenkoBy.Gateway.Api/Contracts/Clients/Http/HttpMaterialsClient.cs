using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Utils;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Extensions;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

public class HttpMaterialsClient : IMaterialsClient
{
    private readonly HttpClientFacade _httpClient;

    private const string BaseMaterialsRoute = "api/materials";

    private const string SerializationErrorMessage = "Произошла ошибка при попытке сериализовать материал.";

    public HttpMaterialsClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<PagedSet<Material>> GetMaterialsAsync(MaterialsFilterDto materialsFilter)
    {
        var response = await _httpClient.GetAsync<ContentListResponse<Material>>(
            BaseMaterialsRoute,
            materialsFilter.ToDictionary()
        );
        return response.ToPagedSet();
    }

    public async Task<Material?> GetMaterialAsync(int materialId)
    {
        try
        {
            var url = GetMaterialRoute(materialId);
            var response = await _httpClient.GetAsync<ContentResponse<Material>>(url);
            return response.Data;
        }
        catch (NotFoundException)
        {
            return null;
        }
    }

    public async Task<Material> CreateMaterialAsync(SaveMaterialDto materialDto)
    {
        var response = await _httpClient.PostAsync<ContentResponse<Material>>(BaseMaterialsRoute, body: materialDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public async Task<Material> UpdateMaterialAsync(int materialId, SaveMaterialDto materialDto)
    {
        var url = GetMaterialRoute(materialId);
        var response = await _httpClient.PatchAsync<ContentResponse<Material>>(url, body: materialDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public Task DeleteMaterial(int materialId) => _httpClient.DeleteAsync(GetMaterialRoute(materialId));

    private static string GetMaterialRoute(int materialId) => $"{BaseMaterialsRoute}/{materialId}";
}