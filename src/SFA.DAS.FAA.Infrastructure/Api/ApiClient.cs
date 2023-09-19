using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.Configuration;
using System.Net;
using System.Text;
using SFA.DAS.FAA.Domain.Interfaces;
using Newtonsoft.Json;

namespace SFA.DAS.FAA.Infrastructure.Api;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly FindAnApprenticeshipApi _config;

    public ApiClient(HttpClient httpClient, IOptions<FindAnApprenticeshipApi> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
        _httpClient.BaseAddress = new Uri(config.Value.BaseUrl);
    }

    public async Task<TResponse> Get<TResponse>(IGetApiRequest request)
    {

        AddHeaders();

        var response = await _httpClient.GetAsync(request.GetUrl).ConfigureAwait(false);

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

    private void AddHeaders()
    {
        _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _config.Key);
        _httpClient.DefaultRequestHeaders.Add("X-Version", "1");
    }
}