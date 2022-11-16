using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web;
using GermonenkoBy.Common.Web.Responses;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Controllers;

[ApiController, Authorize, Route("api/assets")]
public class AssetsController : ControllerBaseWrapper
{
    private readonly IProductAssetsClient _productAssetsClient;

    public AssetsController(IProductAssetsClient productAssetsClient)
    {
        _productAssetsClient = productAssetsClient;
    }

    [HttpGet("")]
    [SwaggerOperation("Search product assets.", "Searches assets using filter parameters provided via query.")]
    [SwaggerResponse(200, "Uploaded product asset.", typeof(ContentListResponse<ProductAsset>))]
    public async Task<ActionResult<ContentListResponse<ProductAsset>>> GetAssetsAsync(
        [FromQuery, SwaggerParameter("Assets filter DTO.")] AssetsFilterDto filterDto
    )
    {
        var assets = await _productAssetsClient.GetAssetsAsync(filterDto);
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
        var asset = await _productAssetsClient.UploadAssetAsync(assetDto);
        return OkWrapped(asset);
    }

    [HttpPatch("{assetId:int}")]
    [SwaggerOperation("Update asset's metadata.", "Updates asset's metadata and returns asset descriptor.")]
    [SwaggerResponse(200, "Updated asset details.", typeof(ContentResponse<ProductAsset>))]
    [SwaggerResponse(400, "Validation error.", typeof(BaseResponse))]
    [SwaggerResponse(404, "Asset not found error.", typeof(BaseResponse))]
    public async Task<ActionResult<ContentResponse<ProductAsset>>> UpdateAssetMetadataAsync(
        [SwaggerParameter("ID of an asset to be updated.")] int assetId,
        [FromBody, SwaggerRequestBody("Asset data to update.")] AssetMetadataDto assetMetadataDto
    )
    {
        var asset = await _productAssetsClient.UpdateAssetMetadataAsync(assetId, assetMetadataDto);
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
        await _productAssetsClient.RemoveAssetAsync(assetId);
        return NoContent();
    }
}