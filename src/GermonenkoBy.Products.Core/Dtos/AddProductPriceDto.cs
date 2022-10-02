using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Products.Core.Models.Enums;

namespace GermonenkoBy.Products.Core.Dtos;

public class AddProductPriceDto
{
    [Range(0, double.MaxValue, ErrorMessage = "Значение цены не может быть отрицательным.")]
    public double Price { get; set; }

    public ProductPriceTypes PriceTypes { get; set; }
}