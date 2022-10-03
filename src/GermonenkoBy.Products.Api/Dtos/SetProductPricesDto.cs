using GermonenkoBy.Products.Core.Dtos;

namespace GermonenkoBy.Products.Api.Dtos;

public class SetProductPricesDto
{
    public ICollection<AddProductPriceDto> ProductPrices { get; set; } = new List<AddProductPriceDto>();
}