using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Enums;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Api.Controllers;

[ApiController, Route("api/materials")]
public class MaterialsController : ControllerBaseWrapper
{
    private readonly MaterialsService _materialsService;

    public MaterialsController(MaterialsService materialsService)
    {
        _materialsService = materialsService;
    }

    [HttpGet("")]
    [SwaggerResponse(200, "List of categories", typeof(ContentListResponse<Material>), ContentTypes.Json)]
    public async Task<ActionResult<ContentListResponse<Material>>> GetCategoriesAsync(
        [SwaggerParameter("material filter."), FromQuery] MaterialsFilterDto filter,
        [FromServices] MaterialsSearchService searchService
    )
    {
        var categories = await searchService.SearchMaterialsAsync(filter);
        return OkWrappedPaged(categories);
    }

    [HttpGet("{materialId:int}")]
    [SwaggerResponse(200, "Material found by ID.", typeof(ContentResponse<Material>), ContentTypes.Json)]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<ContentResponse<Material>>> GetMaterialAsync(
        [SwaggerParameter("ID of a material to search for.")] int materialId
    )
    {
        var material = await _materialsService.GetMaterialAsync(materialId);
        return OkWrapped(material);
    }

    [HttpPost("")]
    [SwaggerResponse(200, "Saved material object.", typeof(ContentResponse<Material>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<Material>> CreateMaterialAsync(
        [FromBody, SwaggerParameter("New material DTO.")] SaveMaterialDto materialDto
    )
    {
        var material = await _materialsService.CreateMaterialAsync(materialDto);
        return OkWrapped(material);
    }

    [HttpPatch("{materialId:int}")]
    [SwaggerResponse(200, "Saved material object.", typeof(ContentResponse<Material>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<Material>> UpdateMaterialAsync(
        [SwaggerParameter("ID of a material to be updated")] int materialId,
        [FromBody, SwaggerParameter("material's data to be updated.")] SaveMaterialDto materialDto
    )
    {
        var material = await _materialsService.UpdateMaterialAsync(materialId, materialDto);
        return OkWrapped(material);
    }

    [HttpDelete("{materialId:int}")]
    [SwaggerResponse(204, contentTypes: ContentTypes.Json)]
    public async Task<NoContentResult> RemoveMaterialAsync(
        [SwaggerParameter("ID of a material to be removed.")] int materialId
    )
    {
        await _materialsService.RemoveMaterialAsync(materialId);
        return NoContent();
    }
}