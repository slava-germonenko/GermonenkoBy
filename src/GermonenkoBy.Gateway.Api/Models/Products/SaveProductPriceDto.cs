using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Gateway.Api.Models.Products.Enums;

namespace GermonenkoBy.Gateway.Api.Models.Products;

public class SaveProductPriceDto
{
    [Range(0, double.MaxValue, ErrorMessage = "Значение цены не может быть отрицательным.")]
    public decimal Price { get; set; }

    public ProductPriceTypes PriceType { get; set; }
}