using GermonenkoBy.Common.Domain;
using GermonenkoBy.Gateway.Api.Models.Products;

namespace GermonenkoBy.Gateway.Api.Contracts.Clients;

public interface IProductAssetsClient
{
    public Task<PagedSet<ProductAsset>> GetAssetsAsync(AssetsFilterDto filter);

    public Task<ProductAsset> CreateAssetAsync(UploadAssetDto assetDto);

    public Task<ProductAsset> UpdateAssetAsync(ModifyAssetDetailsDto assetDetailsDto);

    public Task RemoveAssetAsync(int assetId);
}