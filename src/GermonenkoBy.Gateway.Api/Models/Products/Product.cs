namespace GermonenkoBy.Gateway.Api.Models.Products;

public class Product
{
    public int Id { get; set; }

    public string ItemNumber { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string InternationalName { get; set; } = string.Empty;

    public bool Active { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Material? Material { get; set; }

    public Category? Category { get; set; }

    public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();

    public ICollection<ProductAsset> Assets { get; set; } = new List<ProductAsset>();
}