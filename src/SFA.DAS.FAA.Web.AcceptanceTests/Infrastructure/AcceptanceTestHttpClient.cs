using System.Net;
using Microsoft.Net.Http.Headers;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

public class AcceptanceTestHttpClient(string baseAddress) : ITestHttpClient
{
    private readonly HttpClient _httpClient = new();
    private CookieContainer CookieContainer { get; } = new();

    private HttpRequestMessage BuildRequest(string url, HttpMethod method, HttpContent? content = null)
    {
        var request = new HttpRequestMessage();
        var uri = new Uri(new Uri(baseAddress), url);
        request.RequestUri = uri;

        var cookieHeader = CookieContainer.GetCookieHeader(uri);
        if (!string.IsNullOrWhiteSpace(cookieHeader))
        {
            request.Headers.Add(HeaderNames.Cookie, cookieHeader);
        }

        request.Method = method;

        if (content != null)
        {
            request.Content = content;
        }

        return request;
    }

    private void UpdateCookies(string url, HttpResponseMessage response)
    {
        if (response.Headers.Contains(HeaderNames.SetCookie))
        {
            var uri = new Uri(new Uri(baseAddress), url);
            var cookies = response.Headers.GetValues(HeaderNames.SetCookie);
            foreach (var cookie in cookies)
            {
                CookieContainer.SetCookies(uri, cookie);
            }
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        var request = BuildRequest(url, HttpMethod.Get);
            
        var response = await _httpClient.SendAsync(request);
        UpdateCookies(url, response);
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> content)
    {
        var getResponse = await GetAsync(url);
            
        var html = await getResponse.Content.ReadAsStringAsync();
        var token = ExtractRequestVerificationToken(html);
        content.Add("__RequestVerificationToken", token);
            
        var response = await _httpClient.SendAsync(BuildRequest(url, HttpMethod.Post, new FormUrlEncodedContent(content)));
        UpdateCookies(url, response);
        return response;
    }

    public void Dispose()
    {
    }
        
    private static string ExtractRequestVerificationToken(string html)
    {
        const string tokenFieldName = "\"__RequestVerificationToken\" type=\"hidden\" value=\"";
        var startIndex = html.IndexOf($"{tokenFieldName}", StringComparison.CurrentCultureIgnoreCase) + tokenFieldName.Length;
        var endIndex = html.IndexOf("\"", startIndex, StringComparison.CurrentCultureIgnoreCase);
        return html.Substring(startIndex, endIndex - startIndex);
    }
}