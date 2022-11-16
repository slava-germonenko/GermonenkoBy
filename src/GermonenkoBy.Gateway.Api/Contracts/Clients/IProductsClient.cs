using GermonenkoBy.Common.Domain;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IProductsClient
{
    public Task<PagedSet<Product>> GetProductsAsync(ProductsFilterDto filter);

    public Task<Product?> GetProductAsync(int productId);

    public Task<Product> CreateProductAsync(CreateProductDto productDto);

    public Task<Product> UpdateProductAsync(int productId, ModifyProductDto productDto);

    public Task<Product> SetProductPrices(int productId, ICollection<SaveProductPriceDto> productPrices);

    public Task RemoveProductAsync(int productId);
}