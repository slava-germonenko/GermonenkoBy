using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Controllers;

[ApiController, Authorize, Route("api/materials")]
public class MaterialsController : ControllerBaseWrapper
{
    private readonly IMaterialsClient _materialsClient;

    public MaterialsController(IMaterialsClient materialsClient)
    {
        _materialsClient = materialsClient;
    }

    [HttpGet("")]
    [SwaggerOperation("Search for materials.", "Searches materials using parameters provided via the query.")]
    [SwaggerResponse(200, "List of materials.", typeof(ContentListResponse<Material>))]
    public async Task<ActionResult<ContentListResponse<Material>>> GetMaterialsAsync(
        [FromQuery, SwaggerParameter("Materials filter.")] MaterialsFilterDto filter
    )
    {
        var materials = await _materialsClient.GetMaterialsAsync(filter);
        return OkWrapped(materials);
    }

    [HttpPost("")]
    [SwaggerOperation("Create material.", "Create material using data supplied in the request body.")]
    [SwaggerResponse(200, "Created material.", typeof(ContentResponse<Material>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Material>>> CreateMaterialAsync(
        [FromBody, SwaggerRequestBody("Material data.")] SaveMaterialDto materialDto
    )
    {
        var material = await _materialsClient.CreateMaterialAsync(materialDto);
        return OkWrapped(material);
    }

    [HttpPatch("{materialId:int}")]
    [SwaggerOperation("Update material.", "Update material using data supplied in the request body.")]
    [SwaggerResponse(200, "Updated material.", typeof(ContentResponse<Material>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<Material>>> UpdateMaterialAsync(
        [SwaggerParameter("Material ID.")] int materialId,
        [FromBody, SwaggerRequestBody("Material data.")] SaveMaterialDto materialDto
    )
    {
        var material = await _materialsClient.UpdateMaterialAsync(materialId, materialDto);
        return OkWrapped(material);
    }

    [HttpGet("{materialId:int}")]
    [SwaggerOperation("Get material.", "Tries to find  material by the given ID.")]
    [SwaggerResponse(200, "Material model.", typeof(ContentResponse<Material>))]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse))]
    public async Task<ActionResult> GetMaterialAsync([SwaggerParameter("Material ID")] int materialId)
    {
        var material = await _materialsClient.GetMaterialAsync(materialId);
        return material is null
            ? NotFound(new BaseResponse($"Материал с идентификатором \"{materialId}\" не найден."))
            : OkWrapped(material);
    }

    [HttpDelete("{materialId:int}")]
    [SwaggerOperation("Delete material.", "Removes material by the ID.")]
    [SwaggerResponse(200, "Success response.")]
    public async Task<NoContentResult> RemoveMaterialAsync([SwaggerParameter("Material ID.")] int materialId)
    {
        await _materialsClient.DeleteMaterial(materialId);
        return NoContent();
    }
}