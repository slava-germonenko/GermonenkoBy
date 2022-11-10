namespace GermonenkoBy.Gateway.Api.Models.Products;

public class SetProductPricesDto
{
    public ICollection<SaveProductPriceDto> ProductPrices { get; set; } = new List<SaveProductPriceDto>();
}