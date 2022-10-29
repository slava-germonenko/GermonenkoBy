using System.Net.Http.Json;

using Microsoft.AspNetCore.WebUtilities;

using GermonenkoBy.Common.Web.Extensions;

namespace GermonenkoBy.Common.Web.Http;

/// <summary>
/// .NET HTTP Client Facade that facilitates service-to-service
/// HTTP communications by encapsulating some boilerplate code
/// </summary>
public class HttpClientFacade
{
    private readonly HttpClient _httpClient;

    public string? BaseAddress
    {
        get => _httpClient.BaseAddress?.ToString();
        set => _httpClient.BaseAddress = value is null
            ? null
            : new Uri(value, UriKind.Absolute);
    }

    public HttpClientFacade() : this(new HttpClient()){}

    public HttpClientFacade(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<TResponse> GetAsync<TResponse>(string route, IDictionary<string, string?>? queryParams = null)
        => SendAsync<TResponse>(HttpMethod.Get, route, queryParams);

    public Task<TResponse> PostAsync<TResponse>(
        string route,
        IDictionary<string, string?>? queryParams = null,
        object? body = null
    ) => SendAsync<TResponse>(HttpMethod.Post, route, queryParams, body);

    public Task PostAsync(
        string route,
        IDictionary<string, string?>? queryParams = null,
        object? body = null
    ) => SendAsync(HttpMethod.Post, route, queryParams, body);

    public Task<TResponse> PutAsync<TResponse>(
        string route,
        IDictionary<string, string?>? queryParams = null,
        object? body = null
    ) => SendAsync<TResponse>(HttpMethod.Put, route, queryParams, body);

    public Task PutAsync(
        string route,
        IDictionary<string, string?>? queryParams = null,
        object? body = null
    ) => SendAsync(HttpMethod.Put, route, queryParams, body);

    public Task DeleteAsync(string route, IDictionary<string, string?>? queryParams = null)
        => SendAsync(HttpMethod.Delete, route, queryParams);

    public async Task SendAsync(
        HttpMethod method,
        string route,
        IDictionary<string, string?>? queryParams = null,
        object? body = null
    )
    {
        var uriString = queryParams is null ? route : QueryHelpers.AddQueryString(route, queryParams);
        var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
        var httpRequestMessage = new HttpRequestMessage(method, uri);

        if (body is not null)
        {
            httpRequestMessage.Content = JsonContent.Create(body);
        }

        var responseMessage = await _httpClient.SendAsync(httpRequestMessage);
        if (!responseMessage.IsSuccessStatusCode)
        {
            var error = await HttpExceptionsFactory.CreateAsync(responseMessage);
            throw error;
        }
    }

    public async Task<TResponse> SendAsync<TResponse>(
        HttpMethod method,
        string route,
        IDictionary<string, string?>? queryParams = null,
        object? body = null
    )
    {
        var uriString = queryParams is null ? route : QueryHelpers.AddQueryString(route, queryParams);
        var uri = new Uri(uriString, UriKind.RelativeOrAbsolute);
        var httpRequestMessage = new HttpRequestMessage(method, uri);

        if (body is not null)
        {
            httpRequestMessage.Content = JsonContent.Create(body);
        }

        var responseMessage = await _httpClient.SendAsync(httpRequestMessage);
        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadAsJsonAsync<TResponse>();
        }

        var error = await HttpExceptionsFactory.CreateAsync(responseMessage);
        throw error;
    }
}