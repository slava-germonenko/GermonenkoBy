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

    [HttpGet("")]
    [SwaggerOperation("Search product assets.", "Searches assets using filter parameters provided via query.")]
    [SwaggerResponse(200, "Uploaded product asset.", typeof(ContentListResponse<ProductAsset>))]
    public async Task<ActionResult<ContentListResponse<ProductAsset>>> GetAssetsAsync(
        [FromQuery, SwaggerParameter("Assets filter DTO.")] AssetsFilterDto filterDto,
        [FromServices] AssetsSearchService searchService
    )
    {
        var assets = await searchService.GetProductAssetsAsync(filterDto);
        return OkWrapped(assets);
    }

    [HttpPost("")]
    [SwaggerOperation("Upload new asset.", "Uploads new asset and returns uploaded asset descriptor.")]
    [SwaggerResponse(200, "Uploaded product asset.", typeof(ContentResponse<ProductAsset>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    [SwaggerResponse(404, "Not found error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<ProductAsset>>> UploadAssetAsync(
        [FromBody, SwaggerRequestBody("Asset DTO to be uploaded.")] UploadAssetDto assetDto
    )
    {
        var asset = await _assetsService.UploadAssetAsync(assetDto);
        return OkWrapped(asset);
    }

    [HttpPatch("{assetId:int}")]
    [SwaggerOperation("Update asset's metadata.", "Updates asset's metadata and returns asset descriptor.")]
    [SwaggerResponse(200, "Updated asset details.", typeof(ContentResponse<ProductAsset>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    [SwaggerResponse(404, "Asset not found error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<ProductAsset>>> UpdateAssetDetailsAsync(
        [SwaggerParameter("ID of an asset to be updated.")] int assetId,
        [FromBody, SwaggerRequestBody("Asset data to update.")] ModifyAssetDetailsDto assetDetailsDto
    )
    {
        var asset = await _assetsService.UpdateAssetDetailsAsync(assetId, assetDetailsDto);
        return OkWrapped(asset);
    }

    [HttpDelete("{assetId:int}")]
    [SwaggerOperation(
        "Remove asset.",
        "Removes asset with the given ID. If there is no asset with such ID – nothing happens."
    )]
    [SwaggerResponse(204, "Success response.")]
    public async Task<NoContentResult> RemoveAssetAsync(
        [SwaggerParameter("ID of an asset to be removed.")] int assetId
    )
    {
        await _assetsService.DeleteAssetAsync(assetId);
        return NoContent();
    }
}