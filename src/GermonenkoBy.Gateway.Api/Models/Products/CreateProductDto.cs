namespace GermonenkoBy.Gateway.Api.Models.Products;

public class CreateProductDto : ModifyProductDto
{
    public ICollection<SaveProductPriceDto>? ProductPrices { get; set; }
}