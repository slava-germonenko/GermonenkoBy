using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Utils;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Extensions;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients.Http;

public class HttpProductsClient : IProductsClient
{
    private readonly HttpClientFacade _httpClient;

    private const string BaseMaterialsRoute = "api/products";

    private const string SerializationErrorMessage = "Произошла ошибка при попытке сериализовать модель товара.";

    public HttpProductsClient(HttpClient httpClient)
    {
        _httpClient = new HttpClientFacade(httpClient);
    }

    public async Task<PagedSet<Product>> GetProductsAsync(ProductsFilterDto filter)
    {
        var response = await _httpClient.GetAsync<ContentListResponse<Product>>(
            BaseMaterialsRoute,
            filter.ToDictionary()
        );
        return response.ToPagedSet();
    }

    public async Task<Product?> GetProductAsync(int productId)
    {
        var url = GetProductRoute(productId);
        var response = await _httpClient.GetAsync<ContentResponse<Product>>(url);
        return response.Data;
    }

    public async Task<Product> CreateProductAsync(CreateProductDto productDto)
    {
        var response = await _httpClient.PostAsync<ContentResponse<Product>>(BaseMaterialsRoute, body: productDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public async Task<Product> UpdateProductAsync(int productId, ModifyProductDto productDto)
    {
        var url = GetProductRoute(productId);
        var response = await _httpClient.PatchAsync<ContentResponse<Product>>(url, body: productDto);
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public async Task<Product> SetProductPrices(int productId, ICollection<SaveProductPriceDto> productPrices)
    {
        var url = $"{BaseMaterialsRoute}/{productId}/prices";
        var response = await _httpClient.PutAsync<ContentResponse<Product>>(url, body: new
        {
            productPrices,
        });
        return response.Data ?? throw new Exception(SerializationErrorMessage);
    }

    public Task RemoveProductAsync(int productId)
    {
        var url = GetProductRoute(productId);
        return _httpClient.DeleteAsync(url);
    }

    private static string GetProductRoute(int productId) => $"{BaseMaterialsRoute}/{productId}";
}