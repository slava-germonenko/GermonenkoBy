namespace GermonenkoBy.Common.Domain;

public class PagedSet<TItem> : Paging
{
    public int Total { get; set; }

    public ICollection<TItem> Data { get; set; } = new List<TItem>();
}