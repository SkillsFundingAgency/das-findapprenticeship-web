using System.Net;
using Microsoft.Net.Http.Headers;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

public class AcceptanceTestHttpClient(string baseAddress) : ITestHttpClient
{
    private readonly HttpClient? _httpClient;
    private readonly HttpClientHandler? _httpClientHandler;
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
        //var getResponse = await GetAsync(url);

        //var html = await getResponse.Content.ReadAsStringAsync();
        //var token = ExtractRequestVerificationToken(html);
        //content.Add("__RequestVerificationToken", token);

        //var response = await _httpClient.SendAsync(BuildRequest(url, HttpMethod.Post, new FormUrlEncodedContent(content)));
        //UpdateCookies(url, response);
        //return response;

        // The 'content' dictionary passed into this method (from your test step)
        // should already contain all the necessary form fields, including
        // "Data" and "__RequestVerificationToken".
        // We will now directly use this 'content' dictionary.

        // Optional: Add logging to confirm the content before sending (useful for debugging)
        Console.WriteLine($"DEBUG (AcceptanceTestHttpClient): Sending POST request to: {url}");
        foreach (var item in content)
        {
            Console.WriteLine($"  - Parameter: {item.Key}, Value (first 50 chars): {item.Value.Substring(0, Math.Min(item.Value.Length, 50))}");
        }


        var response = await _httpClient.SendAsync(BuildRequest(url, HttpMethod.Post, new FormUrlEncodedContent(content)));
        UpdateCookies(url, response); // Assuming this method handles cookie updates correctly.
        return response;
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, Dictionary<string, string> formData)
    {
        Console.WriteLine($"DEBUG (AcceptanceTestHttpClient): Sending POST request to: {requestUri}");

        var content = new FormUrlEncodedContent(formData);
        HttpResponseMessage response = null;
        try
        {
            response = await _httpClient.PostAsync(requestUri, content);
            Console.WriteLine($"DEBUG (AcceptanceTestHttpClient): Received response status: {response.StatusCode}");

            // IMPORTANT: Read content here for debugging, even if it's not success
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"DEBUG (AcceptanceTestHttpClient): Actual HTTP Response Body (first 1000 chars from client): {responseBody.Substring(0, Math.Min(responseBody.Length, 1000))}");

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR (AcceptanceTestHttpClient): Exception during POST to {requestUri}: {ex.Message}");
            throw; // Re-throw to propagate the original error
        }
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