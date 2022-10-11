using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Products.Core.Dtos;

public class SaveMaterialDto
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Название материала – обязательное поле.")]
    [StringLength(100, ErrorMessage = "Максимальная длина названия материала – 100 символов.")]
    public string Name { get; set; } = string.Empty;
}