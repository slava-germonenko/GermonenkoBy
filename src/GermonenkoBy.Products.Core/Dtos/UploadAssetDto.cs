using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.Domain.DataAnnotation;

namespace GermonenkoBy.Products.Core.Dtos;

public class UploadAssetDto
{
    public int? ProductId { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Тип mime – обязательное поле.")]
    [MimeTypes(MimeTypes.Image, ErrorMessage = "Тип mime некорректен.")]
    public string MimeType { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Base64 контент – обязательное поле.")]
    public string Base64Content { get; set; } = string.Empty;
}