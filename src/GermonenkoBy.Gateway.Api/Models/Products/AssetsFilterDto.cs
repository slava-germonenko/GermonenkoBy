using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Gateway.Api.Models.Products;

public class AssetsFilterDto : Paging
{
    public int? ProductId { get; set; }

    public bool? Uploaded { get; set; }
}