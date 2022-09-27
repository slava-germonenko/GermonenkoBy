namespace GermonenkoBy.Common.Web.Responses;

public class BaseResponse
{
    public string Message { get; set; } = "Ok.";

    public BaseResponse() { }

    public BaseResponse(string message)
    {
        Message = message;
    }
}