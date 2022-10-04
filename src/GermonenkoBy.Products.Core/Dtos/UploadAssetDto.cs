using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Products.Core.Dtos;

public class UploadAssetDto
{
    public int? ProductId { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string FileName { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Base64Content { get; set; } = string.Empty;
}