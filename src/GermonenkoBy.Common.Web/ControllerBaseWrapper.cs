using Microsoft.AspNetCore.Mvc;

using GermonenkoBy.Common.Domain;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Common.Web;

public class ControllerBaseWrapper : ControllerBase
{
    [NonAction]
    public OkObjectResult OkWrapped(string message)
    {
        return Ok(new BaseResponse(message));
    }

    [NonAction]
    public OkObjectResult OkWrapped<TData>(TData data)
    {
        return Ok(new ContentResponse<TData>(data));
    }

    [NonAction]
    public OkObjectResult OkWrapped<TData>(TData data, string message)
    {
        return Ok(new ContentResponse<TData>(data, message));
    }

    [NonAction]
    public OkObjectResult OkWrapped<TItem>(PagedSet<TItem> pagedSet)
    {
        return Ok(new ContentListResponse<TItem>(pagedSet));
    }

    [NonAction]
    public OkObjectResult OkWrapped<TItem>(PagedSet<TItem> pagedSet, string message)
    {
        return Ok(new ContentListResponse<TItem>(pagedSet, message));
    }
}