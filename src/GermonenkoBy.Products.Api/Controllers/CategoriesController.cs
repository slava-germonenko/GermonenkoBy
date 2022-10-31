using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Enums;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Api.Controllers;

[ApiController, Route("api/categories")]
public class CategoriesController : ControllerBaseWrapper
{
    private readonly CategoriesService _categoriesService;

    public CategoriesController(CategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }

    [HttpGet("")]
    [SwaggerOperation("Categories search.", "Search for categories using filters provided via query params.")]
    [SwaggerResponse(200, "List of categories", typeof(ContentListResponse<Category>), ContentTypes.Json)]
    public async Task<ActionResult<ContentListResponse<Category>>> GetCategoriesAsync(
        [SwaggerParameter("Category filter."), FromQuery] CategoriesFilterDto filter,
        [FromServices] CategoriesSearchService searchService
    )
    {
        var categories = await searchService.SearchCategoriesAsync(filter);
        return OkWrapped(categories);
    }

    [HttpGet("{categoryId:int}")]
    [SwaggerOperation("Get category by ID", "Tries to retrieve category with the provided ID.")]
    [SwaggerResponse(200, "Category found by ID.", typeof(ContentResponse<Category>), ContentTypes.Json)]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<ContentResponse<Category>>> GetCategoryAsync(
        [SwaggerParameter("ID of a category to search for.")] int categoryId
    )
    {
        var category = await _categoriesService.GetCategoryAsync(categoryId);
        return OkWrapped(category);
    }

    [HttpPost("")]
    [SwaggerOperation("Create category.", "Creates new category with the data provided in the body.")]
    [SwaggerResponse(200, "Saved Category object.", typeof(ContentResponse<Category>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<Category>> CreateCategoryAsync(
        [FromBody, SwaggerRequestBody("New category DTO.")] SaveCategoryDto categoryDto
    )
    {
        var category = await _categoriesService.CreateCategoryAsync(categoryDto);
        return OkWrapped(category);
    }

    [HttpPatch("{categoryId:int}")]
    [SwaggerOperation("Update category.", "Updates category with the data provided in the body.")]
    [SwaggerResponse(200, "Saved Category object.", typeof(ContentResponse<Category>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<Category>> UpdateCategoryAsync(
        [SwaggerParameter("ID of a category to be updated")] int categoryId,
        [FromBody, SwaggerRequestBody("Category's data to be updated.")] SaveCategoryDto categoryDto
    )
    {
        var category = await _categoriesService.UpdateCategoryAsync(categoryId, categoryDto);
        return OkWrapped(category);
    }

    [HttpDelete("{categoryId:int}")]
    [SwaggerOperation("Remove category.", "Removes a category with the given ID.")]
    [SwaggerResponse(204, contentTypes: ContentTypes.Json)]
    public async Task<NoContentResult> RemoveCategoryAsync(
        [SwaggerParameter("ID of a category to be removed.")] int categoryId,
        [FromQuery, SwaggerParameter("ID of a category to be assigned to products.")] int? assignTo
    )
    {
        await _categoriesService.RemoveCategoryAsync(categoryId, assignTo);
        return NoContent();
    }
}