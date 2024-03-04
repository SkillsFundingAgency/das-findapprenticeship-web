using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure
{
    public class TestHttpClient : IDisposable
    {
        private TestServer Server { get; }
        private CookieContainer CookieContainer { get; }

        private Uri SubstituteBaseUrl => new Uri("https://www.test.com/");

        public TestHttpClient(TestServer server)
        {
            Server = server;
            CookieContainer = new CookieContainer();
        }

        private RequestBuilder BuildRequest(string url)
        {
            var uri = new Uri(SubstituteBaseUrl, url);
            var builder = Server.CreateRequest(url);

            var cookieHeader = CookieContainer.GetCookieHeader(uri);
            if (!string.IsNullOrWhiteSpace(cookieHeader))
            {
                builder.AddHeader(HeaderNames.Cookie, cookieHeader);
            }

            return builder;
        }

        private void UpdateCookies(string url, HttpResponseMessage response)
        {
            if (response.Headers.Contains(HeaderNames.SetCookie))
            {
                var uri = new Uri(SubstituteBaseUrl, url);
                var cookies = response.Headers.GetValues(HeaderNames.SetCookie);
                foreach (var cookie in cookies)
                {
                    CookieContainer.SetCookies(uri, cookie);
                }
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var response = await BuildRequest(url).GetAsync();
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            var builder = BuildRequest(url);
            builder.And(request => request.Content = content);
            var response = await builder.PostAsync();
            UpdateCookies(url, response);
            return response;
        }

        public void Dispose()
        {
            Server.Dispose();
        }
    }
}
