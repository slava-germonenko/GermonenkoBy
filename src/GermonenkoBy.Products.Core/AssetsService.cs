using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Products.Core.Contracts.Clients;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core;

public class AssetsService
{
    private readonly ProductsContext _context;

    private readonly IAssetsBlobClient _assetsBlobClient;

    private const string Base64ConversionError =
        "Произошла ошибка при попытке декодировать контент файла из base64 строки.";

    private const string AssignedKey = "assigned";

    public AssetsService(
        ProductsContext context,
        IAssetsBlobClient assetsBlobClient
    )
    {
        _context = context;
        _assetsBlobClient = assetsBlobClient;
    }

    public async Task<ProductAsset> UploadAssetAsync(UploadAssetDto assetDto)
    {
        var assetOrder = 0;
        if (assetDto.ProductId is not null)
        {
            await EnsureProductExists(assetDto.ProductId.Value);
            assetOrder = await _context.ProductAssets.CountAsync(p => p.ProductId == assetDto.ProductId.Value);
        }

        var fileContent = ExtractBytes(assetDto.Base64Content);
        var blobName = GenerateBlobName(assetDto.FileName);
        var blobUri = await _assetsBlobClient.UploadAssetAsync(blobName, fileContent);

        var asset = new ProductAsset
        {
            Order = assetOrder,
            ProductId = assetDto.ProductId,
            Size = fileContent.Length,
            FileName = blobName,
            BlobUri = blobUri
        };

        _context.ProductAssets.Add(asset);
        await _context.SaveChangesAsync();

        if (assetDto.ProductId is not null)
        {
            await _assetsBlobClient.SetAssetPropertyAsync(
                blobName,
                AssignedKey,
                assetDto.ProductId.Value.ToString()
            );
        }

        return asset;
    }

    public async Task DeleteAssetAsync(int assetId)
    {
        var asset = await _context.ProductAssets.FindAsync(assetId);
        if (asset is null)
        {
            return;
        }

        if (asset.FileName is not null)
        {
            await _assetsBlobClient.DeleteAssetAsync(asset.FileName);
        }

        _context.ProductAssets.Remove(asset);
        await _context.SaveChangesAsync();
    }

    private async Task EnsureProductExists(int productId)
    {
        var productExists = await _context.Products.AnyAsync(p => p.Id == productId);
        if (!productExists)
        {
            throw new NotFoundException($"Товар и индентификатором \"{productId}\" не найден.");
        }
    }

    private static string GenerateBlobName(string sourceFileName)
    {
        var blobFileName = Guid.NewGuid().ToString().Replace("-", "");
        var fileExt = Path.GetExtension(sourceFileName);
        return $"{blobFileName}{fileExt}";
    }

    private static byte[] ExtractBytes(string base64Content)
    {
        try
        {
            return Convert.FromBase64String(base64Content);
        }
        catch (Exception e)
        {
            throw new CoreLogicException(Base64ConversionError, e);
        }
    }
}