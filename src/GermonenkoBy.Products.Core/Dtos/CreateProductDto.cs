namespace GermonenkoBy.Products.Core.Dtos;

public class CreateProductDto : ModifyProductDto
{
    public ICollection<AddProductPriceDto>? ProductPrices { get; set; }
}