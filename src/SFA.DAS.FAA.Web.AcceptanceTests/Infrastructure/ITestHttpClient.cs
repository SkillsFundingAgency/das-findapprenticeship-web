namespace SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;

public interface ITestHttpClient : IDisposable
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task<HttpResponseMessage> PostAsync(string url, Dictionary<string, string> content);
}