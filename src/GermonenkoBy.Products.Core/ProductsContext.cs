using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Common.EntityFramework;
using GermonenkoBy.Products.Core.Models;
using GermonenkoBy.Products.Core.Models.Configuration;

namespace GermonenkoBy.Products.Core;

public class ProductsContext : BaseContext
{
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Material> Materials => Set<Material>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductAsset> ProductAssets => Set<ProductAsset>();

    public DbSet<ProductPrice> ProductPrices => Set<ProductPrice>();

    public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductAssetEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProductPriceEntityConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}