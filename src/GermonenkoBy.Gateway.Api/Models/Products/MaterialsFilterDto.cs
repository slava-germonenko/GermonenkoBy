using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Gateway.Api.Models.Products;

public class MaterialsFilterDto : Paging
{
    public string? Name { get; set; }

    public string? Search { get; set; }

    public string? OrderBy { get; set; }

    public string OrderDirection = "ASC";
}