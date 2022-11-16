using GermonenkoBy.Common.Domain;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IProductAssetsClient
{
    public Task<PagedSet<ProductAsset>> GetAssetsAsync(AssetsFilterDto filter);

    public Task<ProductAsset> UploadAssetAsync(UploadAssetDto assetDto);

    public Task<ProductAsset> UpdateAssetMetadataAsync(int assetId, AssetMetadataDto assetDetailsDto);

    public Task RemoveAssetAsync(int assetId);
}