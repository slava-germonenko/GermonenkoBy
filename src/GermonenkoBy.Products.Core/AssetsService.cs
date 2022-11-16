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
        var blobName = GenerateBlobName(assetDto.MimeType);
        var blobUri = await _assetsBlobClient.UploadAssetAsync(blobName, fileContent, assetDto.MimeType);

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

    public async Task<ProductAsset> UpdateAssetDetailsAsync(int assetId, AssetMetadataDto assetMetadataDto)
    {
        var assetToUpdate = await _context.ProductAssets.FindAsync(assetId);
        if (assetToUpdate is null)
        {
            throw new NotFoundException($"Ассет с идентификатором \"{assetId}\" не найден.");
        }

        if (assetMetadataDto.ProductId is not null)
        {
            await EnsureProductExists(assetMetadataDto.ProductId.Value);
        }

        assetToUpdate.ProductId = assetMetadataDto.ProductId;
        _context.ProductAssets.Update(assetToUpdate);
        await _context.SaveChangesAsync();

        if (assetToUpdate.FileName is not null && assetMetadataDto.ProductId is not null)
        {
            await _assetsBlobClient.SetAssetPropertyAsync(
                assetToUpdate.FileName,
                AssignedKey,
                assetMetadataDto.ProductId.Value.ToString()
            );
        }

        if (assetToUpdate.FileName is not null && assetMetadataDto.ProductId is null)
        {
            await _assetsBlobClient.SetAssetPropertyAsync(
                assetToUpdate.FileName,
                AssignedKey,
                null
            );
        }

        return assetToUpdate;
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
            throw new CoreLogicException($"Товар и индентификатором \"{productId}\" не найден.");
        }
    }

    private static string GenerateBlobName(string mimeType)
    {
        if (!MimeTypesToFileExtMap.TryGetValue(mimeType, out var fileExt))
        {
            throw new CoreLogicException($"Файлы типа \"{mimeType}\" не поддерживаются.");
        }

        var blobFileName = Guid.NewGuid().ToString().Replace("-", "");
        return $"{blobFileName}.{fileExt}";
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

    private static readonly Dictionary<string, string> MimeTypesToFileExtMap = new(StringComparer.OrdinalIgnoreCase)
    {
        {"image/avif", "avif"},
        {"image/bmp", "bmp"},
        {"image/vnd.microsoft.icon", "ico"},
        {"image/jpeg", "jpeg"},
        {"image/png", "png"},
        {"image/xml+svg", "svg"},
        {"image/tiff", "tiff"},
        {"image/webp", "webp"},
        {"image/gif", "gif"},
    };
}