using GermonenkoBy.Gateway.Api.Models.Products.Enums;

namespace GermonenkoBy.Gateway.Api.Models.Products;

public class ProductPrice
{
    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public ProductPriceTypes PriceType { get; set; }
}