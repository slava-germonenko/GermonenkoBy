using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GermonenkoBy.Products.Core.Models.Configuration;

public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasMany(p => p.Prices)
            .WithOne()
            .HasForeignKey(pp => pp.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Material)
            .WithMany()
            .HasForeignKey("MaterialId");

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey("CategoryId");
    }
}