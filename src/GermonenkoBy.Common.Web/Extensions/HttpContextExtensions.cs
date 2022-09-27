using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

using Microsoft.AspNetCore.Http;

namespace GermonenkoBy.Common.Web.Extensions;

public static class HttpContextExtensions
{
    private static readonly JsonSerializerOptions _serializationOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
    };

    public static ValueTask WriteJsonResponse(
        this HttpContext context,
        HttpStatusCode statusCode,
        object body
    )
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.Headers.ContentType = "application/json";
        var bodyBytes = JsonSerializer.SerializeToUtf8Bytes(body, _serializationOptions);
        return context.Response.Body.WriteAsync(bodyBytes);
    }
}