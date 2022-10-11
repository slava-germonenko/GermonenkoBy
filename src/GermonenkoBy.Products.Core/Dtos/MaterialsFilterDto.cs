using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Products.Core.Dtos;

public class MaterialsFilterDto : Paging
{
    public string? Name { get; set; }

    public string? Search { get; set; }

    public string? OrderBy { get; set; }

    public string OrderDirection = "ASC";
}