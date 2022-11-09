namespace GermonenkoBy.Gateway.Api.Models.Products;

public class CreateProductDto : ModifyProductDto
{
    public ICollection<AddProductPriceDto>? ProductPrices { get; set; }
}