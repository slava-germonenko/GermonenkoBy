using System.Net;

using Microsoft.AspNetCore.Http;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Common.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ExceptionHandlingBehavior _exceptionHandlingBehavior;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ExceptionHandlingBehavior exceptionHandlingBehavior = ExceptionHandlingBehavior.NoCatch
    )
    {
        _next = next;
        _exceptionHandlingBehavior = exceptionHandlingBehavior;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (CoreLogicException e)
        {
            var response = new BaseResponse(e.Message);
            await context.WriteJsonResponse(HttpStatusCode.BadRequest, response);
        }
        catch (NotFoundException e)
        {
            var response = new BaseResponse(e.Message);
            await context.WriteJsonResponse(HttpStatusCode.NotFound, response);
        }
        catch (Exception e)
        {
            await HandleError(context, e);
        }
    }

    private ValueTask HandleError(HttpContext context, Exception exception)
    {
        switch (_exceptionHandlingBehavior)
        {
            case ExceptionHandlingBehavior.RethrowGenericError:
            {
                var errorResponse = new BaseResponse("Произошла непредвиденная ошибка!");
                return context.WriteJsonResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
            case ExceptionHandlingBehavior.RethrowDetailedError:
            {
                var errorResponse = new DevErrorResponse(exception.Message, exception.StackTrace);
                return context.WriteJsonResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
            default:
            {
                throw exception;
            }
        }
    }
}