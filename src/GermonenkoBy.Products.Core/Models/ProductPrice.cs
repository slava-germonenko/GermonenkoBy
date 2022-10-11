using GermonenkoBy.Products.Core.Models.Enums;

namespace GermonenkoBy.Products.Core.Models;

public class ProductPrice
{
    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public ProductPriceTypes PriceType { get; set; }
}