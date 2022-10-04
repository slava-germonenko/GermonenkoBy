using System.ComponentModel.DataAnnotations;

namespace GermonenkoBy.Products.Core.Models;

public class ProductAsset
{
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    public int? ProductId { get; set; }

    public string? FileName { get; set; }

    public Uri? BlobUri { get; set; }

    public long Size { get; set; }
}