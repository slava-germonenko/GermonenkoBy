using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Utils;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Extensions;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

public class HttpProductAssetsClient : IProductAssetsClient
{
    private readonly HttpClientFacade _httpClient;

    private const string BaseAssetsRoute = "api/assets";

    private const string SerializationErrorMessage = "Произошла ошибка при попытке сериализовать ассет.";

    public HttpProductAssetsClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<PagedSet<ProductAsset>> GetAssetsAsync(AssetsFilterDto filter)
    {
        var response = await _httpClient.GetAsync<ContentListResponse<ProductAsset>>(
            BaseAssetsRoute,
            filter.ToDictionary()
        );
        return response.ToPagedSet();
    }

    public async Task<ProductAsset> UploadAssetAsync(UploadAssetDto assetDto)
    {
        var response = await _httpClient.PostAsync<ContentResponse<ProductAsset>>(BaseAssetsRoute, body: assetDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public async Task<ProductAsset> UpdateAssetMetadataAsync(int assetId, AssetMetadataDto assetDetailsDto)
    {
        var url = GetAssetRoute(assetId);
        var response = await _httpClient.PatchAsync<ContentResponse<ProductAsset>>(url, body: assetDetailsDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public Task RemoveAssetAsync(int assetId) => _httpClient.DeleteAsync(GetAssetRoute(assetId));

    private static string GetAssetRoute(int assetId) => $"{BaseAssetsRoute}/{assetId}";
}