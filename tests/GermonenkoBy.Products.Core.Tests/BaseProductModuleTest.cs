using Microsoft.EntityFrameworkCore;

namespace GermonenkoBy.Products.Core.Tests;

public abstract class BaseProductModuleTest
{
    protected static ProductsContext CreateInMemoryContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductsContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new ProductsContext(optionsBuilder.Options);
    }
}