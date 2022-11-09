namespace GermonenkoBy.Gateway.Api.Models.Products;

public class Material
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}