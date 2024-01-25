using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.Configuration;
using System.Net;
using SFA.DAS.FAA.Domain.Interfaces;
using System.Text;
using Newtonsoft.Json;
using SFA.DAS.FAA.Domain;
using SFA.DAS.FAA.Domain;
using Newtonsoft.Json.Serialization;

namespace SFA.DAS.FAA.Infrastructure.Api;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly FindAnApprenticeshipOuterApi _config;

    public ApiClient(HttpClient httpClient, IOptions<FindAnApprenticeshipOuterApi> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
        _httpClient.BaseAddress = new Uri(config.Value.BaseUrl);
    }

    public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, request.GetUrl);
        AddAuthenticationHeader(requestMessage);

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        if (response.StatusCode.Equals(HttpStatusCode.NotFound))
        {
            return default;
        }

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);
        }

        response.EnsureSuccessStatusCode();

        return default;
    }

    public async Task<TResponse> Put<TResponse>(IPutApiRequest request)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, request.PutUrl)
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request.Data), Encoding.UTF8, "application/json"),
            VersionPolicy = HttpVersionPolicy.RequestVersionOrLower
        };
        AddAuthenticationHeader(requestMessage);

        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);

        if (response.StatusCode.Equals(HttpStatusCode.NotFound))
        {
            return default;
        }

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<TResponse>(json);

        }

        response.EnsureSuccessStatusCode();
        return default;
    }

    public async Task<TResponse?> PostWithResponseCode<TResponse>(IPostApiRequest request)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl)
        {
            Content = stringContent,
        };
        AddAuthenticationHeader(requestMessage);
        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonConvert.DeserializeObject<TResponse>(responseContent) ?? default;
    }

    public async Task PostWithResponseCode(IPostApiRequest request)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, request.PostUrl)
        {
            Content = stringContent,
        };
        AddAuthenticationHeader(requestMessage);
        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    public async Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, request.PutUrl)
        {
            Content = stringContent,
        };
        AddAuthenticationHeader(requestMessage);
        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var apiResponse = new ApiResponse<TResponse>(JsonConvert.DeserializeObject<TResponse>(responseContent), response.StatusCode, null);

        return apiResponse;
    }

    public async Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request)
    {
        var stringContent = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, request.PutUrl)
        {
            Content = stringContent,
        };
        AddAuthenticationHeader(requestMessage);
        var response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var apiResponse = new ApiResponse<TResponse>(JsonConvert.DeserializeObject<TResponse>(responseContent), response.StatusCode, null);

        return apiResponse;
    }

    private void AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _config.Key);
        httpRequestMessage.Headers.Add("X-Version", "1");
    }
}