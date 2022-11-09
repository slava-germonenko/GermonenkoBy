using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Gateway.Api.Models.Products;

public class CategoriesFilterDto : Paging
{
    public string? Name { get; set; }

    public string? Search { get; set; }

    public string? OrderBy { get; set; }

    public string OrderDirection { get; set; } = "ASC";
}