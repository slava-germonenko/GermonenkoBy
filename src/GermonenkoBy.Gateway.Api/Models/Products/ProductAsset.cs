namespace GermonenkoBy.Gateway.Api.Models.Products;

public class ProductAsset
{
    public int Id { get; set; }

    public int Order { get; set; }

    public int? ProductId { get; set; }

    public string? FileName { get; set; }

    public Uri? BlobUri { get; set; }

    public long Size { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}