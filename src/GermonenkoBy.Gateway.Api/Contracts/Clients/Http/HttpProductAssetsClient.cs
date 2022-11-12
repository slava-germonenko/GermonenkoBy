using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

public class HttpProductAssetsClient : IProductAssetsClient
{
    private readonly HttpClientFacade _httpClient;

    private const string BaseMaterialsRoute = "api/assets";

    private const string SerializationErrorMessage = "Произошла ошибка при попытке сериализовать ассет.";

    public HttpProductAssetsClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public Task<PagedSet<ProductAsset>> GetAssetsAsync(AssetsFilterDto filter)
    {
        throw new NotImplementedException();
    }

    public Task<ProductAsset> CreateAssetAsync(UploadAssetDto assetDto)
    {
        throw new NotImplementedException();
    }

    public Task<ProductAsset> UpdateAssetAsync(ModifyAssetDetailsDto assetDetailsDto)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAssetAsync(int assetId)
    {
        throw new NotImplementedException();
    }
}