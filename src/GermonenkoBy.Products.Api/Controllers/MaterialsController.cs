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
    [SwaggerOperation("Material search.", "Search for materials using filters provided via query params.")]
    [SwaggerResponse(200, "List of materials.", typeof(ContentListResponse<Material>), ContentTypes.Json)]
    public async Task<ActionResult<ContentListResponse<Material>>> GetCategoriesAsync(
        [SwaggerParameter("material filter."), FromQuery] MaterialsFilterDto filter,
        [FromServices] MaterialsSearchService searchService
    )
    {
        var categories = await searchService.SearchMaterialsAsync(filter);
        return OkWrapped(categories);
    }

    [HttpGet("{materialId:int}")]
    [SwaggerOperation("Get material by ID", "Tries to retrieve material with the provided ID.")]
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
    [SwaggerOperation("Create material.", "Creates new material with the data provided in the body.")]
    [SwaggerResponse(200, "Saved material object.", typeof(ContentResponse<Material>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<Material>> CreateMaterialAsync(
        [FromBody, SwaggerRequestBody("New material DTO.")] SaveMaterialDto materialDto
    )
    {
        var material = await _materialsService.CreateMaterialAsync(materialDto);
        return OkWrapped(material);
    }

    [HttpPatch("{materialId:int}")]
    [SwaggerOperation("Update material.", "Updates material with the data supplied in the request body.")]
    [SwaggerResponse(200, "Saved material object.", typeof(ContentResponse<Material>), ContentTypes.Json)]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse), ContentTypes.Json)]
    public async Task<ActionResult<Material>> UpdateMaterialAsync(
        [SwaggerParameter("ID of a material to be updated")] int materialId,
        [FromBody, SwaggerRequestBody("material's data to be updated.")] SaveMaterialDto materialDto
    )
    {
        var material = await _materialsService.UpdateMaterialAsync(materialId, materialDto);
        return OkWrapped(material);
    }

    [HttpDelete("{materialId:int}")]
    [SwaggerOperation("Remove material.", "Removes material with the given ID.")]
    [SwaggerResponse(204, contentTypes: ContentTypes.Json)]
    public async Task<NoContentResult> RemoveMaterialAsync(
        [SwaggerParameter("ID of a material to be removed.")] int materialId
    )
    {
        await _materialsService.RemoveMaterialAsync(materialId);
        return NoContent();
    }
}