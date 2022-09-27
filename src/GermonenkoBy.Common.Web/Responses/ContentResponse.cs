namespace GermonenkoBy.Common.Web.Responses;

public class ContentResponse<TData> : BaseResponse
{
    public TData? Data { get; set; }

    public ContentResponse() { }

    public ContentResponse(TData data)
    {
        Data = data;
    }

    public ContentResponse(TData data, string message)
    {
        Data = data;
        Message = message;
    }
}