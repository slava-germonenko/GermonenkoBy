using System.Net.Http.Json;
using System.Runtime.Serialization;

namespace GermonenkoBy.Common.Web.Extensions;

public static class HttpContentExtensions
{
    public static async Task<TResponse> ReadAsJsonAsync<TResponse>(this HttpContent httpContent)
    {
        var responseData = await httpContent.ReadFromJsonAsync<TResponse>();
        if (responseData is null)
        {
            var responseStr = await httpContent.ReadAsStringAsync();
            throw new SerializationException(
                $"Failed to serialize {typeof(TResponse).FullName} the following response \"{responseStr}\"."
            );
        }

        return responseData;
    }
}