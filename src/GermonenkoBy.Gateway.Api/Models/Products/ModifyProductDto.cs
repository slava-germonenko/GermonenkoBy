using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Gateway.Api.Models.Products;

public class ModifyProductDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Артикул – обязательное поле.")]
    [StringLength(100, ErrorMessage = "Максимальная длина артикула – 100 символов.")]
    [RegularExpression(@"\w+-\d+", ErrorMessage = "Артикул должен быть в формате АА-0000.")]
    public string ItemNumber { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "Максимальная длина наменования товара – 100 символов.")]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [StringLength(100, ErrorMessage = "Максимальная длина международного наменования товара – 100 символов.")]
    public string InternationalName { get; set; } = string.Empty;

    public bool Active { get; set; } = true;

    public int? MaterialId { get; set; }

    public int? CategoryId { get; set; }
}