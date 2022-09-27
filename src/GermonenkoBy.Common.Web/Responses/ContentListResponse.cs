using GermonenkoBy.Common.Domain;

namespace GermonenkoBy.Common.Web.Responses;

public class ContentListResponse<TItem> : ContentResponse<ICollection<TItem>>
{
    public int Total { get; set; }

    public int Count { get; set; }

    public int Offset { get; set; }

    public ContentListResponse() { }

    public ContentListResponse(PagedSet<TItem> pagedSet)
    {
        Data = pagedSet.Data;
        Count = pagedSet.Count;
        Offset = pagedSet.Offset;
        Total = pagedSet.Total;
    }

    public ContentListResponse(PagedSet<TItem> pagedSet, string message)
    {
        Data = pagedSet.Data;
        Count = pagedSet.Count;
        Offset = pagedSet.Offset;
        Total = pagedSet.Total;
        Message = message;
    }
}