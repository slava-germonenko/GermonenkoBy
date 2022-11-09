using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Controllers;

[ApiController, Authorize, Produces("application/json"), Route("api/categories")]
public class CategoriesController : ControllerBaseWrapper
{
    private readonly ICategoriesClient _categoriesClient;

    public CategoriesController(ICategoriesClient categoriesClient)
    {
        _categoriesClient = categoriesClient;
    }

    [HttpGet("")]
    [SwaggerOperation("Get categories.", "Searches categories using parameter given via the query.")]
    [SwaggerResponse(200, "List of categories.", typeof(ContentListResponse<Category>))]
    public async Task<ActionResult<ContentListResponse<Category>>> GetCategoriesAsync(
        [FromQuery, SwaggerParameter("Categories filter.")] CategoriesFilterDto filter
    )
    {
        var categories = await _categoriesClient.GetCategoriesAsync(filter);
        return OkWrapped(categories);
    }

    [HttpGet("{categoryId:int}")]
    [SwaggerOperation(
        "Get category.",
        "Tries to get category by the given ID. Throws error if category is not found."
    )]
    [SwaggerResponse(200, "Found category.", typeof(ContentResponse<Category>))]
    [SwaggerResponse(404, "Not found error.", typeof(ContentResponse<Category>))]
    public async Task<ActionResult<ContentResponse<Category>>> GetCategoryAsync(
        [SwaggerParameter("Category ID")] int categoryId
    )
    {
        var category = await _categoriesClient.GetCategoryAsync(categoryId);
        return OkWrapped(category);
    }

    [HttpPost("")]
    [SwaggerOperation("Create category.", "Creates category using data supplied in the body.")]
    [SwaggerResponse(200, "Created category.", typeof(ContentResponse<Category>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Category>>> CreateCategoryAsync(
        [FromBody, SwaggerRequestBody("Category data")] SaveCategoryDto categoryDto
    )
    {
        var category = await _categoriesClient.CreateCategoryAsync(categoryDto);
        return OkWrapped(category);
    }

    [HttpPatch("{categoryId:int}")]
    [SwaggerOperation("Update category.", "Updates category using data supplied in the body.")]
    [SwaggerResponse(200, "Updated category.", typeof(ContentResponse<Category>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Category>>> UpdateCategoryAsync(
        [SwaggerParameter("Category ID")] int categoryId,
        [FromBody, SwaggerRequestBody("Category data")] SaveCategoryDto categoryDto
    )
    {
        var category = await _categoriesClient.UpdateCategoryAsync(categoryId, categoryDto);
        return OkWrapped(category);
    }

    [HttpDelete("{categoryId:int}")]
    [SwaggerOperation("Remove category.", "Removes category by the provided ID.")]
    [SwaggerResponse(204, "No content success response.")]
    public async Task<NoContentResult> RemoveCategoryAsync([SwaggerParameter("Category ID.")] int categoryId)
    {
        await _categoriesClient.DeleteCategoryAsync(categoryId);
        return NoContent();
    }
}