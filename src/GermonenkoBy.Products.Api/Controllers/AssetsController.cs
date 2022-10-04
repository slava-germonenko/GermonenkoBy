using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Api.Controllers;

[ApiController, Route("api/assets")]
public class AssetsController : ControllerBaseWrapper
{
    private readonly AssetsService _assetsService;

    public AssetsController(AssetsService assetsService)
    {
        _assetsService = assetsService;
    }

    [HttpPost("")]
    [SwaggerResponse(200, "Uploaded product asset.", typeof(ContentResponse<ProductAsset>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<ProductAsset>>> UploadAssetAsync(
        [FromBody, SwaggerParameter("Asset DTO to be uploaded.")] UploadAssetDto assetDto
    )
    {
        var asset = await _assetsService.UploadAssetAsync(assetDto);
        return OkWrapped(asset);
    }

    [HttpDelete("{assetId:int}")]
    [SwaggerResponse(204, "Success response.")]
    public async Task<NoContentResult> RemoveAssetAsync(
        [SwaggerParameter("ID of an asset to be removed.")] int assetId
    )
    {
        await _assetsService.DeleteAssetAsync(assetId);
        return NoContent();
    }
}