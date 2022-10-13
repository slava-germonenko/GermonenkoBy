using System.Net;
using System.Net.Http.Json;

using GermonenkoBy.Common.Domain.Exceptions;
using GermonenkoBy.Common.Web.Responses;

namespace GermonenkoBy.Common.Web.Http;

public static class HttpExceptionsFactory
{
    public static async Task<Exception> CreateAsync(HttpResponseMessage responseMessage)
    {
        if (responseMessage.IsSuccessStatusCode)
        {
            throw new Exception("Unable to create exception for a non-error http response.");
        }

        var error = await responseMessage.Content.ReadFromJsonAsync<BaseResponse>();
        var errorMessage = error is null ? await responseMessage.Content.ReadAsStringAsync() : error.Message;

        return responseMessage.StatusCode switch
        {
            HttpStatusCode.BadRequest => new CoreLogicException(errorMessage),
            HttpStatusCode.Unauthorized => new UnauthorizedAccessException(errorMessage),
            HttpStatusCode.NotFound => new NotFoundException(errorMessage),
            _ => new Exception(errorMessage)
        };
    }
}