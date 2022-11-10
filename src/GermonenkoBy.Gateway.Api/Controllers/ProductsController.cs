using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Controllers;

[ApiController, Authorize, Produces("application/json"), Route("api/products")]
public class ProductsController : ControllerBaseWrapper
{
    private readonly IProductsClient _productsClient;

    public ProductsController(IProductsClient productsClient)
    {
        _productsClient = productsClient;
    }

    [HttpGet("")]
    [SwaggerOperation("Get products", "Searches for products using filters passed via the query.")]
    [SwaggerResponse(200, "List of products.", typeof(ContentListResponse<Product>))]
    public async Task<ActionResult<ContentListResponse<Product>>> GetProductsAsync(
        [FromQuery, SwaggerParameter("Products filter")] ProductsFilterDto filter
    )
    {
        var products = await _productsClient.GetProductsAsync(filter);
        return OkWrapped(products);
    }

    [HttpPost("")]
    [SwaggerOperation("Crate product.", "Creates product using data supplied in the request body.")]
    [SwaggerResponse(200, "Created product", typeof(ContentResponse<Product>))]
    [SwaggerResponse(400, "Validation error", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Product>>> CreateProductAsync(
        [FromBody, SwaggerRequestBody("Product model")] CreateProductDto productDto
    )
    {
        var product = await _productsClient.CreateProductAsync(productDto);
        return OkWrapped(product);
    }

    [HttpGet("{productId:int}")]
    [SwaggerOperation("Get product", "Tries to get product by the ID. If nothing is found, it will throw the error.")]
    [SwaggerResponse(200, "Found product", typeof(ContentResponse<Product> ))]
    [SwaggerResponse(404, "Not found error", typeof(BaseResponse))]
    public async Task<ActionResult> GetProductAsync(
        [SwaggerParameter("Product ID")] int productId
    )
    {
        var product = await _productsClient.GetProductAsync(productId);
        if (product is null)
        {
            return NotFound(new BaseResponse($"Товар с индентификатором {productId} не найден."));
        }
        return OkWrapped(product);
    }

    [HttpPatch("{productId:int}")]
    [SwaggerOperation("Crate product.", "Creates product using data supplied in the request body.")]
    [SwaggerResponse(200, "Updated product", typeof(ContentResponse<Product>))]
    [SwaggerResponse(400, "Validation error", typeof(BaseResponse))]
    [SwaggerResponse(404, "Not found error", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Product>>> UpdateProductAsync(
        [SwaggerParameter("ID of a product to be updated.")] int productId,
        [FromBody, SwaggerRequestBody("Product model")] ModifyProductDto productDto
    )
    {
        var product = await _productsClient.UpdateProductAsync(productId, productDto);
        return OkWrapped(product);
    }

    [HttpDelete("{productId:int}")]
    [SwaggerResponse(204, "Success response")]
    public async Task<NoContentResult> RemoveProductAsync(
        [SwaggerParameter("ID of a product to be removed")] int productId
    )
    {
        await _productsClient.RemoveProductAsync(productId);
        return NoContent();
    }

    [HttpPut("{productId:int}/prices")]
    [SwaggerResponse(200, "Updated product", typeof(ContentResponse<Product>))]
    [SwaggerResponse(400, "Validation error", typeof(BaseResponse))]
    [SwaggerResponse(404, "Product not found error", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Product>>> SetProductPrices(
        [SwaggerParameter("ID of a product to be updated")] int productId,
        [FromBody, SwaggerRequestBody("Prices to be set")] SetProductPricesDto productPrices
    )
    {
        var product = await _productsClient.SetProductPrices(productId, productPrices.ProductPrices);
        return OkWrapped(product);
    }
}