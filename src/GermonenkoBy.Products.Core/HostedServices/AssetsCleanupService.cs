using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.HostedServices;

namespace GermonenkoBy.Products.Core.HostedServices;

public class AssetsCleanupService : IRunnableHostedService
{
    private readonly ProductsContext _context;

    private const int ProcessLimit = 500;

    public AssetsCleanupService(ProductsContext context)
    {
        _context = context;
    }

    public async Task RunAsync()
    {
        var assetsToCleaUpDateThreshold = DateTime.UtcNow.AddDays(-1);

        var assetsToCleanUp = await _context.ProductAssets
            .Where(asset =>
                (asset.FileName == null || asset.BlobUri == null || asset.ProductId == null)
                && asset.CreatedDate <= assetsToCleaUpDateThreshold
            )
            .ToListAsync();

        _context.ProductAssets.RemoveRange(assetsToCleanUp);
    }
}