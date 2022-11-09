using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Gateway.Api.Models.Products;

public class ProductsFilterDto : Paging
{
    public string? Name { get; set; }

    public string? ItemNumber { get; set; }

    public string? InternationalName { get; set; }

    public bool? Active { get; set; }

    public int? MaterialId { get; set; }

    public int? CategoryId { get; set; }

    public string? Search { get; set; }

    public string? OrderBy { get; set; }

    public string OrderDirection = "ASC";
}