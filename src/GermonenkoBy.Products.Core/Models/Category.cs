using System.ComponentModel.DataAnnotations;

using GermonenkoBy.Common.EntityFramework.Models;

namespace GermonenkoBy.Products.Core.Models;

public class Category : IChangeDateTrackingModel
{
    [Key]
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false), StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}