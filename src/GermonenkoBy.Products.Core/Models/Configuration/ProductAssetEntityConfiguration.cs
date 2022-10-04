using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GermonenkoBy.Products.Core.Models.Configuration;

public class ProductAssetEntityConfiguration : IEntityTypeConfiguration<ProductAsset>
{
    public void Configure(EntityTypeBuilder<ProductAsset> builder)
    {
        builder.Property(asset => asset.BlobUri)
            .HasConversion(
                uri => uri == null ? null : uri.ToString(),
                urlStr => urlStr == null ? null : new Uri(urlStr)
            );

        builder.Property(asset => asset.Order)
            .HasColumnName("AssetOrder");
    }
}