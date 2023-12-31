﻿using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.Configuration;
using System.Net;
using SFA.DAS.FAA.Domain.Interfaces;
using Newtonsoft.Json;

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

    private void AddAuthenticationHeader(HttpRequestMessage httpRequestMessage)
    {
        httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _config.Key);
        httpRequestMessage.Headers.Add("X-Version", "1");
    }
}