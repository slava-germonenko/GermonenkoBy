using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Enums;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Products.Api.Dtos;
using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Api.Controllers;

[ApiController, Route("api/products")]
public class ProductsController : ControllerBaseWrapper
{
    private readonly ProductsService _productsService;

    public ProductsController(ProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet("")]
    [SwaggerOperation("Products search.", "Search for products using filters provided via query params.")]
    [SwaggerResponse(200, "List of products.", typeof(ContentListResponse<Product>), ContentTypes.Json)]
    public async Task<ActionResult<ContentListResponse<Product>>> GetProductsAsync(
        [FromQuery, SwaggerParameter("Products filter.")] ProductsFilterDto filterDto,
        [FromServices] ProductsSearchService searchService
    )
    {
        var products = await searchService.SearchProductsAsync(filterDto);
        return OkWrapped(products);
    }

    [HttpPost("")]
    [SwaggerOperation("Create product.", "Creates new product with the data provided in the body.")]
    [SwaggerResponse(200, "Created Product.", typeof(ContentResponse<Product>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<ContentResponse<Product>>> CreateProductAsync(
        [FromBody, SwaggerRequestBody("Product DTO.")] CreateProductDto productDto
    )
    {
        var product = await _productsService.CreateProductAsync(productDto);
        return OkWrapped(product);
    }

    [HttpGet("{productId:int}")]
    [SwaggerOperation("Get product by ID", "Tries to retrieve product with the provided ID.")]
    [SwaggerResponse(200, "Product found by ID.", typeof(ContentResponse<Product>), ContentTypes.Json)]
    [SwaggerResponse(400, "Not found error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<ContentResponse<Product>>> GetProductAsync(
        [SwaggerParameter("ID of a product to get.")] int productId
    )
    {
        var product = await _productsService.GetProductAsync(productId);
        return OkWrapped(product);
    }

    [HttpPatch("{productId:int}")]
    [SwaggerOperation("Update product.", "Updates product with the data supplied in the request body.")]
    [SwaggerResponse(200, "Updated Product.", typeof(ContentResponse<Product>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<ContentResponse<Product>>> UpdateProductAsync(
        [SwaggerParameter("ID of a product to be updated.")] int productId,
        [FromBody, SwaggerRequestBody("Product DTO.")] ModifyProductDto productDto
    )
    {
        var product = await _productsService.UpdateProductDetails(productId, productDto);
        return OkWrapped(product);
    }

    [HttpDelete("{productId:int}")]
    [SwaggerOperation("Remove product.", "Removes product with the given ID.")]
    [SwaggerResponse(204, "Success response.")]
    public async Task<NoContentResult> RemoveProductAsync(int productId)
    {
        await _productsService.RemoveProductAsync(productId);
        return NoContent();
    }

    [HttpPut("{productId:int}/prices")]
    [SwaggerOperation("Set product prices.", "Sets (overwrites) prices assigned to a product.")]
    [SwaggerResponse(200, "Updated product.", typeof(ContentResponse<Product>), ContentTypes.Json)]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<ContentResponse<Product>>> SetProductPricesAsync(
        [SwaggerParameter("Product ID to updated.")] int productId,
        [FromBody, SwaggerRequestBody("Product prices to be st.")] SetProductPricesDto pricesDto
    )
    {
        var product = await _productsService.SetProductPricesAsync(productId, pricesDto.ProductPrices);
        return OkWrapped(product);
    }
}