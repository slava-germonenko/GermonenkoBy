using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.EntityFramework.Extensions;
using GermonenkoBy.Products.Core.Dtos;
using GermonenkoBy.Products.Core.Models;

namespace GermonenkoBy.Products.Core;

public class AssetsSearchService
{
    private readonly ProductsContext _context;

    public AssetsSearchService(ProductsContext context)
    {
        _context = context;
    }

    public Task<PagedSet<ProductAsset>> GetProductAssetsAsync(AssetsFilterDto assetsFilter)
    {
        var assetsQuery = _context.ProductAssets.AsNoTracking();

        if (assetsFilter.Uploaded is not null)
        {
            assetsQuery = assetsQuery.Where(asset => asset.BlobUri == null || asset.FileName == null);
        }

        if (assetsFilter.ProductId is not null)
        {
            assetsQuery = assetsQuery.Where(asset => asset.ProductId == assetsFilter.ProductId);
        }

        return assetsQuery.OrderByDescending(asset => asset.CreatedDate).ToPagedSetAsync(assetsFilter);
    }
}