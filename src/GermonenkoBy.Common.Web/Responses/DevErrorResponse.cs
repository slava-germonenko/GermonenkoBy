namespace GermonenkoBy.Common.Web.Responses;

public class DevErrorResponse : BaseResponse
{
    public string? StackTrace { get; set; }

    public DevErrorResponse() { }

    public DevErrorResponse(string message, string? stackTrace) : base(message)
    {
        StackTrace = stackTrace;
    }
}