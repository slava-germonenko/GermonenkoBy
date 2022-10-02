using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Products.Core.Models;

public class Product : IChangeDateTrackingModel
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(100)]
    public string ItemNumber { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false), StringLength(100)]
    public string InternationalName { get; set; } = string.Empty;

    public bool Active { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Material? Material { get; set; }

    public Category? Category { get; set; }

    public ICollection<ProductPrice> ProductPrices { get; set; } = new List<ProductPrice>();
}