using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Products.Core.Dtos;

public class AssetsFilterDto : Paging
{
    public int? ProductId { get; set; }

    public bool? Uploaded { get; set; }
}