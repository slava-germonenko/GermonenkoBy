using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Products.Core.Models;

public class ProductAsset : IChangeDateTrackingModel
{
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int Order { get; set; }

    public int? ProductId { get; set; }

    public string? FileName { get; set; }

    public Uri? BlobUri { get; set; }

    public long Size { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}