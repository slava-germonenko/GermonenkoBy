using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GermonenkoBy.Products.Core.Models.Configuration;

public class ProductPriceEntityConfiguration : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
        builder.HasKey(pp => new { pp.ProductId, pp.PriceType });
    }
}